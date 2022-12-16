﻿namespace DeliveryVHGP.Core.Models
{
    public class StoreModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? BrandStoreId { get; set; }
        public string? BrandStoreName { get; set; }
        public string? Phone { get; set; }
        public string? BuildingId { get; set; }

        public string? BuildingStore { get; set; }

        public string? StoreCateId { get; set; }
        public string? StoreCateName { get; set; }
        public string? Password { get; set; }
        public bool? Status { get; set; }
        public double? CommissionRate { get; set; }
        public string? CreateAt { get; set; }
        public string? UpdateAt { get; set; }
        public AccountInRole Account { get; set; }
    }
    public class FilterRequestInStore
    {
        public string? SearchByStoreName { get; set; } = "";
        public string? SearchByBuilding { get; set; } = "";
        public string? SearchByStoreCategory { get; set; } = "";
        public string? SearchByBrand { get; set; } = "";
    }
    public class WalletsStoreModel
    {
        public double? CommissionBalance { get; set; }
    }
}
