using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DeliveryVHGP.Core.Entities;

namespace DeliveryVHGP.Core.Data
{
    public partial class DeliveryVHGP_DBContext : DbContext
    {
        public DeliveryVHGP_DBContext()
        {
        }

        public DeliveryVHGP_DBContext(DbContextOptions<DeliveryVHGP_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<ActionType> ActionTypes { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategoryInMenu> CategoryInMenus { get; set; } = null!;
        public virtual DbSet<Cluster> Clusters { get; set; } = null!;
        public virtual DbSet<Collection> Collections { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<DeliveryShiftOfShipper> DeliveryShiftOfShippers { get; set; } = null!;
        public virtual DbSet<DeliveryTimeFrame> DeliveryTimeFrames { get; set; } = null!;
        public virtual DbSet<FcmToken> FcmTokens { get; set; } = null!;
        public virtual DbSet<Hub> Hubs { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderAction> OrderActions { get; set; } = null!;
        public virtual DbSet<OrderActionHistory> OrderActionHistories { get; set; } = null!;
        public virtual DbSet<OrderCache> OrderCaches { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderTask> OrderTasks { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductInCollection> ProductInCollections { get; set; } = null!;
        public virtual DbSet<ProductInMenu> ProductInMenus { get; set; } = null!;
        public virtual DbSet<ProductTag> ProductTags { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RouteEdge> RouteEdges { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<Segment> Segments { get; set; } = null!;
        public virtual DbSet<SegmentDeliveryRoute> SegmentDeliveryRoutes { get; set; } = null!;
        public virtual DbSet<SegmentTask> SegmentTasks { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<Shift> Shifts { get; set; } = null!;
        public virtual DbSet<Shipper> Shippers { get; set; } = null!;
        public virtual DbSet<ShipperHistory> ShipperHistories { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<StoreCategory> StoreCategories { get; set; } = null!;
        public virtual DbSet<StoreInMenu> StoreInMenus { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:vinhome-fooddelivery.database.windows.net,1433;Initial Catalog=DeliveryVHGP_DB;Persist Security Info=False;User ID=vhgp;Password=Admin123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.RoleId).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<ActionType>(entity =>
            {
                entity.ToTable("ActionType");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Building");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.ClusterId).HasMaxLength(50);

                entity.Property(e => e.HubId).HasMaxLength(50);

                entity.Property(e => e.Latitude).HasMaxLength(250);

                entity.Property(e => e.Longitude).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Cluster)
                    .WithMany(p => p.Buildings)
                    .HasForeignKey(d => d.ClusterId)
                    .HasConstraintName("FK_Building_Cluster");

                entity.HasOne(d => d.Hub)
                    .WithMany(p => p.Buildings)
                    .HasForeignKey(d => d.HubId)
                    .HasConstraintName("FK_Building_Hub");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CreateAt).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasMaxLength(50);
            });

            modelBuilder.Entity<CategoryInMenu>(entity =>
            {
                entity.ToTable("CategoryInMenu");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CategoryId).HasMaxLength(50);

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryInMenus)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_CategoryInMenu_Category");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.CategoryInMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_CategoryInMenu_Menu");
            });

            modelBuilder.Entity<Cluster>(entity =>
            {
                entity.ToTable("Cluster");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.AreaId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Clusters)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Cluster_Area");
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("Collection");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.StoreId).HasMaxLength(50);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Collections)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Collection_Store");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BuildingId).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_Customer_Building");
            });

            modelBuilder.Entity<DeliveryShiftOfShipper>(entity =>
            {
                entity.ToTable("DeliveryShiftOfShipper");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.HubId).HasMaxLength(50);

                entity.Property(e => e.ScheduleId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ShiftId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ShipperId).HasMaxLength(50);

                entity.Property(e => e.TaskType)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.DeliveryShiftOfShippers)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK_DeliveryShift_Schedule");

                entity.HasOne(d => d.Shift)
                    .WithMany(p => p.DeliveryShiftOfShippers)
                    .HasForeignKey(d => d.ShiftId)
                    .HasConstraintName("FK_DeliveryShift_Shift");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.DeliveryShiftOfShippers)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_DeliveryShift_Shipper");
            });

            modelBuilder.Entity<DeliveryTimeFrame>(entity =>
            {
                entity.ToTable("DeliveryTimeFrame");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.DeliveryTimeFrames)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_DeliveryTimeFrame_Menu");
            });

            modelBuilder.Entity<FcmToken>(entity =>
            {
                entity.ToTable("FcmToken");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.AccountId).HasMaxLength(50);

                entity.Property(e => e.Token)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.FcmTokens)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_FcmToken_Account");
            });

            modelBuilder.Entity<Hub>(entity =>
            {
                entity.ToTable("Hub");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BuildingId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.DayFilter).HasColumnType("date");

                entity.Property(e => e.EndDate).HasMaxLength(20);

                entity.Property(e => e.HourFilter).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.SaleMode).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasMaxLength(20);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Notification");

                entity.Property(e => e.Date)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Image)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.NotiContent)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Time)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Title)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Notification_Account");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BuildingId).HasMaxLength(50);

                entity.Property(e => e.CustomerId).HasMaxLength(50);

                entity.Property(e => e.DeliveryTimeId).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.ServiceId).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasMaxLength(50);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_Order_Building");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.DeliveryTime)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliveryTimeId)
                    .HasConstraintName("FK_Order_DeliveryTimeFrame");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_Order_Menu");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Order_Service");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Order_Store");
            });

            modelBuilder.Entity<OrderAction>(entity =>
            {
                entity.ToTable("OrderAction");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.RouteEdgeId).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderActions)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderAction_Order");

                entity.HasOne(d => d.RouteEdge)
                    .WithMany(p => p.OrderActions)
                    .HasForeignKey(d => d.RouteEdgeId)
                    .HasConstraintName("FK_OrderAction_RouteEdge");
            });

            modelBuilder.Entity<OrderActionHistory>(entity =>
            {
                entity.ToTable("OrderActionHistory");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.TypeId).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderActionHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderActionHistory_Order");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.OrderActionHistories)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_OrderActionHistory_ActionType");
            });

            modelBuilder.Entity<OrderCache>(entity =>
            {
                entity.ToTable("OrderCache");

                entity.HasIndex(e => e.OrderId, "UQ__OrderCac__C3905BCEF6F86C79")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithOne(p => p.OrderCache)
                    .HasForeignKey<OrderCache>(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderCache_Order");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasMaxLength(50);

                entity.Property(e => e.ProductName).HasMaxLength(100);

                entity.Property(e => e.Quantity).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<OrderTask>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OrderTask");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.ShipperId).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Task).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderTask_Order");

                entity.HasOne(d => d.Shipper)
                    .WithMany()
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_OrderTask_Shipper");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasMaxLength(100);

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Payment_Order");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CategoryId).HasMaxLength(50);

                entity.Property(e => e.CreateAt).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PackDescription).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasMaxLength(50);

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Product_ProductCategory");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Product_ShopOwner");
            });

            modelBuilder.Entity<ProductInCollection>(entity =>
            {
                entity.ToTable("ProductInCollection");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CollectionId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProductId).HasMaxLength(50);

                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.ProductInCollections)
                    .HasForeignKey(d => d.CollectionId)
                    .HasConstraintName("FK_ProductInCollection_Collection");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductInCollections)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductInCollection_Product");
            });

            modelBuilder.Entity<ProductInMenu>(entity =>
            {
                entity.ToTable("ProductInMenu");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasMaxLength(50);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_ProductInMenu_Menu");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductInMenu_Product");
            });

            modelBuilder.Entity<ProductTag>(entity =>
            {
                entity.ToTable("ProductTag");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasMaxLength(50);

                entity.Property(e => e.TagId).HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductTag_Product");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_ProductTag_Tag");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RouteEdge>(entity =>
            {
                entity.ToTable("RouteEdge");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.FromBuildingId).HasMaxLength(50);

                entity.Property(e => e.RouteId).HasMaxLength(50);

                entity.Property(e => e.ToBuildingId).HasMaxLength(50);

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteEdges)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("FK_RouteEdge_SegmentDeliveryRoute");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Day)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Month)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Year)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Segment>(entity =>
            {
                entity.ToTable("Segment");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.FromBuildingId).HasMaxLength(50);

                entity.Property(e => e.HubId).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.SegmentTaskId).HasMaxLength(50);

                entity.Property(e => e.ToBuildingId).HasMaxLength(50);

                entity.HasOne(d => d.FromBuilding)
                    .WithMany(p => p.Segments)
                    .HasForeignKey(d => d.FromBuildingId)
                    .HasConstraintName("FK_Segment_Building");

                entity.HasOne(d => d.Hub)
                    .WithMany(p => p.Segments)
                    .HasForeignKey(d => d.HubId)
                    .HasConstraintName("FK_Segment_Hub");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Segments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Segment_Order");

                entity.HasOne(d => d.SegmentTask)
                    .WithMany(p => p.Segments)
                    .HasForeignKey(d => d.SegmentTaskId)
                    .HasConstraintName("FK_Segment_SegmentTask");
            });

            modelBuilder.Entity<SegmentDeliveryRoute>(entity =>
            {
                entity.ToTable("SegmentDeliveryRoute");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.FromShipperId).HasMaxLength(50);

                entity.Property(e => e.ShipperId).HasMaxLength(50);

                entity.Property(e => e.ToShipperId).HasMaxLength(50);

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.SegmentDeliveryRoutes)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_SegmentDeliveryRoute_Shipper");
            });

            modelBuilder.Entity<SegmentTask>(entity =>
            {
                entity.ToTable("SegmentTask");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.ShipperId).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.SegmentTasks)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_SegmentTask_Shipper");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.EndTime)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.StartTime)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.ToTable("Shipper");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Colour).HasMaxLength(50);

                entity.Property(e => e.CreateAt).HasMaxLength(50);

                entity.Property(e => e.DeliveryTeam).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.LicensePlates).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasMaxLength(50);

                entity.Property(e => e.VehicleType).HasMaxLength(50);
            });

            modelBuilder.Entity<ShipperHistory>(entity =>
            {
                entity.ToTable("ShipperHistory");

                entity.Property(e => e.Id).HasMaxLength(50);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BrandId).HasMaxLength(50);

                entity.Property(e => e.BuildingId).HasMaxLength(50);

                entity.Property(e => e.CloseTime).HasMaxLength(50);

                entity.Property(e => e.CreateAt).HasMaxLength(50);

                entity.Property(e => e.CreditAccount).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.OpenTime).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Rate)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Slogan).HasMaxLength(50);

                entity.Property(e => e.StoreCategoryId).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasMaxLength(50);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_ShopOwner_Brand");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_Store_Building");

                entity.HasOne(d => d.StoreCategory)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.StoreCategoryId)
                    .HasConstraintName("FK_Store_StoreCategory");
            });

            modelBuilder.Entity<StoreCategory>(entity =>
            {
                entity.ToTable("StoreCategory");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<StoreInMenu>(entity =>
            {
                entity.ToTable("StoreInMenu");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasMaxLength(50);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.StoreInMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_StoreInMenu_Menu");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreInMenus)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_StoreInMenu_Store");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
