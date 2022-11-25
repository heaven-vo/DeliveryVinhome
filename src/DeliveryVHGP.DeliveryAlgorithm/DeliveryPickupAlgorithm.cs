using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.DeliveryAlgorithm.Model;
using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryVHGP.DeliveryAlgorithm
{
    public class DeliveryPickupAlgorithm
    {
        private readonly IServiceProvider _serviceProvider;
        public DeliveryPickupAlgorithm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void AlgorithsProcess(List<SegmentModel> listSegments)
        {
            //List<SegmentModel> list = new List<SegmentModel>() { new SegmentModel(){fromBuilding = "b1", toBuilding = "b3"},
            //                                                     new SegmentModel(){fromBuilding = "b5", toBuilding = "b6"},
            //                                                    new SegmentModel(){fromBuilding = "b5", toBuilding = "b6"}};
            // Instantiate the data problem.
            DataModel data = new DataModel();
            data.VehicleNumber = 4;
            data.PickupsDeliveriesData = ChangeBuildingIdIntoInt(listSegments); //{1,6},{2,6},{1,6},{2,1} 
            foreach (var a in data.PickupsDeliveriesData)
            {
                Console.WriteLine(a[0] + " " + a[1]);
            }
            data.ListNodes = ChangeListSegmetToNode(data.PickupsDeliveriesData); //{0,1,2,6}
            data.DistanceMatrix = Convert(data.DistanceMatrixData, data.ListNodes); //new matrix
            data.ListNodeWithNewIndex = ChangeNodesIntoNewIndex(data.ListNodes); //{0,1,2,3}
            List<NodesMapping> map = new List<NodesMapping>();           //real:{0,1,2,6}, fake:{0,1,2,3}
            for (int i = 0; i < data.ListNodes.Length; i++)
            {
                NodesMapping node = new NodesMapping() { RealNode = data.ListNodes[i], FakeNode = data.ListNodeWithNewIndex[i] };
                map.Add(node);
                //Console.WriteLine(node.RealNode + " " + node.FakeNode);
            }
            data.NodesMappings = map;
            data.PickupsDeliveries = ChangeSegmentToNew(data.PickupsDeliveriesData, map); //{1,6},{2,6},{1,6},{2,1} ->{1,3},{2,3},{1,3},{2,1}
            // Create Routing Index Manager
            RoutingIndexManager manager =
                new RoutingIndexManager(data.DistanceMatrix.GetLength(0), data.VehicleNumber, data.Depot);


            // Create Routing Model.
            RoutingModel routing = new RoutingModel(manager);

            // Create and register a transit callback.
            int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
            {
                // Convert from routing variable Index to
                // distance matrix NodeIndex.
                var fromNode = manager.IndexToNode(fromIndex);
                var toNode = manager.IndexToNode(toIndex);
                return data.DistanceMatrix[fromNode, toNode];
            });

            // Define cost of each arc.
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            // Add Distance constraint.
            routing.AddDimension(transitCallbackIndex, 0, 50000,
                                 true, // start cumul to zero
                                 "Distance");
            RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");
            distanceDimension.SetGlobalSpanCostCoefficient(100);

            // Define Transportation Requests.
            Solver solver = routing.solver();
            for (int i = 0; i < data.PickupsDeliveries.GetLength(0); i++)
            {
                long pickupIndex = manager.NodeToIndex(data.PickupsDeliveries[i][0]);
                long deliveryIndex = manager.NodeToIndex(data.PickupsDeliveries[i][1]);
                routing.AddPickupAndDelivery(pickupIndex, deliveryIndex);
                solver.Add(solver.MakeEquality(routing.VehicleVar(pickupIndex), routing.VehicleVar(deliveryIndex)));
                solver.Add(solver.MakeLessOrEqual(distanceDimension.CumulVar(pickupIndex),
                                                  distanceDimension.CumulVar(deliveryIndex)));
            }

            // Setting first solution heuristic.
            RoutingSearchParameters searchParameters =
                operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
            searchParameters.TimeLimit = new Duration { Seconds = 5 };

            // Solve the problem.
            Assignment solution = routing.SolveWithParameters(searchParameters);

            // Print solution on console.
            PrintSolution(data, routing, manager, solution, listSegments);
        }
        public async void PrintSolution(DataModel data, RoutingModel routing, RoutingIndexManager manager,
                               Assignment solution, List<SegmentModel> listSegments)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IRepositoryWrapper>();
                // Inspect solution.----------------------------------------------------
                long maxRouteDistance = 0;
                //long totalRouteDistance = 0;
                var enumCount = System.Enum.GetNames(typeof(BuildingEnum)).Length;
                List<SegmentDeliveryRoute> listRoute = new List<SegmentDeliveryRoute>();
                List<NodeModel> NodeAction = new List<NodeModel>();                     // check fake node(delivery, >n)
                Console.WriteLine("Matrix lenght {0}:", data.DistanceMatrixData.Length);
                for (int i = 0; i < data.VehicleNumber; ++i)
                {
                    long totalRouteDistance = 0;
                    Console.WriteLine("Route for Vehicle {0}:", i);
                    // Creat table Delivery Route
                    SegmentDeliveryRoute route = new SegmentDeliveryRoute() { Id = Guid.NewGuid().ToString(), Status = (int)RouteStatusEnum.NotAssign };
                    List<RouteEdge> listEdge = new List<RouteEdge>();
                    List<NodeModel> listNode = new List<NodeModel>();
                    long routeDistance = 0;
                    var start = routing.Start(i);
                    var index = solution.Value(routing.NextVar(start));
                    string previousBuiding = "";
                    int priority = 1;
                    while (routing.IsEnd(index) == false)
                    {
                        RouteEdge edge = new RouteEdge() { Id = Guid.NewGuid().ToString(), FromBuildingId = previousBuiding, Status = (int)EdgeStatusEnum.NotYet };
                        NodeModel node = new NodeModel() { EdgeId = edge.Id, Type = (int)EdgeTypeEnum.Pickup };
                        //Console.Write("{0} -> ", manager.IndexToNode((int)index));
                        int buildingEnum = data.NodesMappings.Where(x => x.FakeNode == index).Select(x => x.RealNode).FirstOrDefault();//new code
                        if (buildingEnum > enumCount)
                        {
                            buildingEnum -= enumCount;
                            node.Type = (int)EdgeTypeEnum.Delivery;
                        }
                        string buildId = ((BuildingEnum)buildingEnum).ToString();
                        previousBuiding = buildId;
                        Console.Write("{0} -> ", buildingEnum);
                        //        //Create Route edge
                        //        //List order Id -> list segment -> check building id -> create orderAction()
                        //        //if segment type 2(hub - cus), 3(store - cus) -> remove order Queue
                        //        //if segment type 1(store - hub) -> remove then add to queue || do nothing

                        //        //load list order from queue -> segment(not done, done) -> 1 segment then remove order from queue 
                        //        //-> segment done -> order done, fail or at hub
                        //        //if order at hub -> add order to queue

                        var previousIndex = index;
                        index = solution.Value(routing.NextVar(index));
                        routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);

                        edge.ToBuildingId = buildId;
                        edge.Priority = priority;
                        node.ToBuildingId = buildId;
                        if (priority == 1)
                        {
                            edge.Distance = 0;
                            //edge.Status = (int)EdgeStatusEnum.ToDo;
                        }
                        else
                            edge.Distance = routeDistance;
                        listEdge.Add(edge);
                        priority++;
                        listNode.Add(node);
                    }
                    Console.WriteLine("{0}", manager.IndexToNode((int)index));
                    Console.WriteLine("Distance of the route: {0}m", routeDistance);
                    totalRouteDistance += routeDistance;
                    route.Distance = totalRouteDistance;
                    route.RouteEdges = listEdge;
                    if (totalRouteDistance > 0)
                    {
                        listRoute.Add(route);
                        NodeAction.AddRange(listNode);
                    }
                }
                Console.WriteLine("Segment of the route: {0}", listSegments.Count);

                await repo.RouteAction.CreateRoute(listRoute, listSegments);
                await repo.RouteAction.CreateActionOrder(NodeAction, listSegments);
                //Console.WriteLine("Maximum distance of the routes: {0}m", maxRouteDistance);
            }
        }
        //Ulity___________________________________________________________________________________________-

        // list Segment -> int[][] vector for algorithms input db -> {1,6},{2,6},{1,6},{2,1}
        public int[][] ChangeBuildingIdIntoInt(List<SegmentModel> input)
        {
            var enumCount = System.Enum.GetNames(typeof(BuildingEnum)).Length;
            Console.WriteLine("Enum count: " + enumCount);
            int[][] result = new int[input.Count()][];
            var buildings = System.Enum.GetValues(typeof(BuildingEnum))
                        .Cast<BuildingEnum>()
                        .Select(d => (d, (int)d))
                        .ToList();

            int i = 0;
            foreach (var bu in input)
            {
                int[] a = new int[2];
                foreach (var build in buildings)
                {
                    if (build.Item1.ToString() == bu.fromBuilding)
                    {
                        a[0] = build.Item2;
                        //Console.WriteLine(build.Item2);
                    }
                    else if (build.Item1.ToString() == bu.toBuilding)
                    {
                        a[1] = build.Item2 + enumCount; //+n(n: building)
                        //Console.WriteLine(build.Item2);
                    }

                }
                result[i] = a;
                i++;
            }
            return result;
        }
        //Remove duplicate and sort and take node need to pass {1,6},{2,6},{1,6},{2,1} -> {0,1,2,6} add 0 to list
        public int[] ChangeListSegmetToNode(int[][] input)
        {

            List<int> nodes = new List<int>();
            foreach (var node in input)
            {
                nodes.Add(node[0]);
                nodes.Add(node[1]);
            }
            List<int> newNode = nodes.Distinct().ToList();
            newNode.Sort();

            int[] result = new int[newNode.Count() + 1];
            //add fake node 0
            result[0] = 0;
            for (int i = 1; i < newNode.Count() + 1; i++)
            {
                result[i] = newNode[i - 1];
            }

            return result;
        }
        //remove node in distance matrix, just take node in segment   take{0,1,2,6}-> new matrix inpt for algorithms  
        public int[,] Convert(int[,] DistanceMatrixAllNode, int[] NeedNode)
        {
            int n = NeedNode.GetLength(0);
            int[,] DistanceMatrixNeedNode = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    DistanceMatrixNeedNode[i, j] = DistanceMatrixAllNode[NeedNode[i], NeedNode[j]];
                    Console.Write(DistanceMatrixNeedNode[i, j] + " ");
                }
                Console.WriteLine("");
            }
            return DistanceMatrixNeedNode;
        }
        //changes node index after changes matrix distance {0,1,3,6} -> {0,1,2,3}
        public int[] ChangeNodesIntoNewIndex(int[] input)
        {
            //Console.WriteLine(input.Length);
            int[] result = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = i;
            }
            return result;
        }
        public int[][] ChangeSegmentToNew(int[][] segment, List<NodesMapping> map)
        {
            //var result = new int[segment.Length][];
            for (int i = 0; i < segment.Length; i++)
            {
                for (int j = 0; j < segment[i].Length; j++)
                {
                    segment[i][j] = map.Where(x => x.RealNode == segment[i][j]).Select(x => x.FakeNode).FirstOrDefault();
                }
            }
            return segment;
        }
    }
}
