using System;

namespace SupplyMonitor.Models
{
    internal class Orders
    {
        public string fruitName { get; set; }
        public string supplierName { get; set; }
        public int weight { get; set; }
        public int totalPrice { get; set; }
        public DateTime dateTime { get; set; }
    }
}
