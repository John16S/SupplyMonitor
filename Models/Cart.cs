using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyMonitor.Models
{
    internal class Cart
    {
        string suplplier;
        int idSupplier;
        string fruit;
        int idFruit;
        int price;
        int weight;
        int totalPrice;

        public string Suplplier { get => suplplier; set => suplplier = value; }
        public int IdSupplier { get => idSupplier; set => idSupplier = value; }
        public string Fruit { get => fruit; set => fruit = value; }
        public int IdFruit { get => idFruit; set => idFruit = value; }
        public int Price { get => price; set => price = value; }
        public int Weight { get => weight; set => weight = value; }
        public int TotalPrice { get => totalPrice; set => totalPrice = value; }

        public void print()
        {
            MessageBox.Show("IdSupplier: " + idSupplier + "\n" +
                            "IdFruit: " + idFruit + "\n");
        }
    }
}
