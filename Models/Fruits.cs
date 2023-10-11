using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyMonitor.Models
{
    internal class Fruits
    {
        public int Id { get; set; }
        public int IdSupplier { get; set; }
        public string Fruit_Name { get; set; }
        public int Price { get; set; }
    }
}
