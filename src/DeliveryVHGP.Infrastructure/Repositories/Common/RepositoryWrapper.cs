using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Infrastructure.Services;
using DeliveryVHGP.WebApi.Repositories;

namespace DeliveryVHGP.Infrastructure.Repositories.Common
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DeliveryVHGP_DBContext _context;
        private readonly IFileService _fileService;
        private readonly ITimeStageService _timeStageService;
        private readonly IFirestoreService _firestoreService;
        private readonly INotificationService _notificationService;

        public RepositoryWrapper(DeliveryVHGP_DBContext context, IFileService fileService, ITimeStageService timeStageService, IFirestoreService firestoreService, INotificationService notificationService)
        {
            _context = context;
            _fileService = fileService;
            _timeStageService = timeStageService;
            _firestoreService = firestoreService;
            _notificationService = notificationService;
            Menu = new MenuRepository(_context);
            Account = new AccountRepository(_context);
            Area = new AreaRepositore(_context);
            Brand = new BrandRepository(_context);
            Hub = new HubRepository(_context);
            Building = new BuildingRepository(_context);
            Category = new CategoriesRepository(_fileService, _context);
            Collection = new CollectionRepository(_context);
            Customer = new CustomerRepository(_fileService, _context);
            Order = new OrdersRepository(_context, _firestoreService);
            Product = new ProductRepository(_timeStageService, _fileService, _context);
            Shipper = new ShipperRepository(_timeStageService, _fileService, _context);
            Store = new StoreRepository(_timeStageService, _fileService, _context);
            StoreCategory = new StoreCategoryRepository(_context);
            Segment = new SegmentRepository(_context);
            Cache = new CacheRepository(_context);
            RouteAction = new RouteActionRepository(_context, _firestoreService, _notificationService);
            ShipperHistory = new ShipperHistoryRepository(_context);
            Transaction = new TransactionRepository(_context);
            _notificationService = notificationService;
        }
        public IMenuRepository Menu { get; private set; }

        public IAccountRepository Account { get; private set; }

        public IAreaRepository Area { get; private set; }

        public IBrandRepository Brand { get; private set; }
        public IHubRepository Hub { get; private set; }

        public IBuildingRepository Building { get; private set; }

        public ICategoriesRepository Category { get; private set; }

        public ICollectionRepository Collection { get; private set; }

        public ICustomerRepository Customer { get; private set; }

        public IOrderRepository Order { get; private set; }

        public IProductRepository Product { get; private set; }

        public IShipperRepository Shipper { get; private set; }

        public IStoreRepository Store { get; private set; }

        public IStoreCategoryRepository StoreCategory { get; private set; }
        public ISegmentRepository Segment { get; private set; }
        public ICacheRepository Cache { get; private set; }
        public IRouteActionRepository RouteAction { get; private set; }
        public IShipperHistoryRepository ShipperHistory { get; private set; }
        public ITransactionRepository Transaction { get; private set; }
    }
}
