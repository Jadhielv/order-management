using System;

namespace OrderManagement.Core.Models
{
    public class OrderStatistics
    {
        public int Id { get; set; }
        public int TotalOrders { get; set; }
        public int DailyOrders { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DailyAmount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 