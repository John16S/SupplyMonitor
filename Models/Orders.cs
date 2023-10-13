using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
