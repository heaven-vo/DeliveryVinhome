using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.DeliveryAlgorithm.Model
{
    public class DataModel
    {
        public int[,] DistanceMatrixData = {
            { 0, 0, 0, 0, 0, 0},
            { 0, 0, 684, 308, 194, 502 },
            { 0, 684, 0, 992, 878, 502 },
            { 0, 308, 992, 0, 114, 650},
            { 0, 194, 878, 114, 0, 536 },
            { 0, 502, 502, 650, 536, 0},
        };
        public int[,] DistanceMatrix { get; set; }
        public int[][] PickupsDeliveriesData { get; set; }
        public int[][] PickupsDeliveries { get; set; }
        public int[] ListNodes { get; set; }
        public int[] ListNodeWithNewIndex { get; set; }
        public List<NodesMapping> NodesMappings { get; set; }
        public int VehicleNumber { get; set; }
        public int Depot = 0;
    }
}
