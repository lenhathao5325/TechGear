using System.Collections.Generic;

namespace TechGear.Models.ViewModels
{
    public class AdminDashboardVM
    {
        public decimal TotalSales { get; set; }
        public int OrdersCount { get; set; }
        public int ProductsCount { get; set; }
        public int UsersCount { get; set; }

        public List<RecentOrderItem> RecentOrders { get; set; } = new List<RecentOrderItem>();

        public List<LowStockItem> LowStockProducts { get; set; } = new List<LowStockItem>();
    }

    public class RecentOrderItem
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }

    public class LowStockItem
    {
        public string Name { get; set; }
        public int Left { get; set; }
    }
}
