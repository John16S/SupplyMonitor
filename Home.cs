using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using SupplyMonitor.DBConnections;
using SupplyMonitor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;

namespace SupplyMonitor
{
    public partial class Home : Form
    {

        List<Cart> order = new List<Cart>();

        public Home()
        {
            InitializeComponent();

            LoadSuppliers loadSuppliers = new LoadSuppliers();

            Task<List<Supplier>> getDataTask = Task.Run(() => loadSuppliers.GetSuppliers());
            getDataTask.ContinueWith(task =>
            {
                if (task.Result.Count > 0)
                {
                    supplierBox.DataSource = task.Result;

                    supplierBox.ValueMember = "Id";
                    supplierBox.DisplayMember = "Name";
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void RecalculateTotalPrice()
        {
            // Получаем текст из priceTextBox и Разделяем строку по пробелу, то есть из "120 р/кг" берем "120"
            string priceText = CutTheCrapMadaFaka(priceTextBox);
            
            if (priceText.Length > 0 && int.TryParse(priceText, out int price) && decimal.TryParse(amountNumericUpDown.Value.ToString(), out decimal amount))
            {
                decimal totalPrice = price * (int)amount;
                totalPriceTextBox.Text = totalPrice.ToString() + " руб.";
            }
            else
            {
                // Обработка ошибки, если введены неверные значения
                totalPriceTextBox.Text = "Ошибка";
            }
        }

        private void priceTextBox_TextChanged(object sender, EventArgs e)
        {
            RecalculateTotalPrice();
        }

        private void amountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            RecalculateTotalPrice();
        }

        private void supplierBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем выбранного поставщика
            Supplier selectedSupplier = supplierBox.SelectedItem as Supplier;

            if (selectedSupplier != null)
            {
                int idSupplier = selectedSupplier.Id;

                // Выполняем запрос к базе данных, чтобы получить фрукты для выбранного поставщика
                LoadFruits loadFruits = new LoadFruits();
                List<Fruits> fruits = loadFruits.GetFruits(idSupplier.ToString());

                // Загружаем фрукты в typeOfFriutBox
                foreach (Fruits fruit in fruits)
                {
                    typeOfFriutBox.DataSource = fruits;

                    typeOfFriutBox.ValueMember = "Id";
                    typeOfFriutBox.DisplayMember = "Fruit_Name";
                }
            }
        }

        private void typeOfFriutBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            priceTextBox.Clear();
            Fruits selectedFruit = typeOfFriutBox.SelectedItem as Fruits;

            if (selectedFruit != null)
            {
                // Отобразить цену выбранного фрукта в priceTextBox
                priceTextBox.Text = selectedFruit.Price.ToString() + " р/кг";
            }
        }

        private void addToCartBtn_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            cart.Suplplier = supplierBox.Text;
            cart.IdSupplier = int.TryParse((supplierBox.SelectedValue.ToString()), out int idsupplier) ? idsupplier : 0;
            cart.Fruit = typeOfFriutBox.Text;
            cart.IdFruit = int.TryParse((typeOfFriutBox.SelectedValue.ToString()), out int idfruit) ? idsupplier : 0;
            cart.Price = int.TryParse(CutTheCrapMadaFaka(priceTextBox), out int price) ? price : 0;
            cart.Weight = (int)amountNumericUpDown.Value;
            cart.TotalPrice = int.TryParse(CutTheCrapMadaFaka(totalPriceTextBox), out int totalPrice) ? totalPrice : 0;

            order.Add(cart);

            dataGridView1.Rows.Add(cart.Suplplier, cart.Fruit, cart.Price, cart.Weight, cart.TotalPrice);
            cart.print();
        }

        /// <summary>
        /// Обрезает всю не нужную бобуйню
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns>TextBox</returns>
        private string CutTheCrapMadaFaka(TextBox textBox)
        {
            string[] splittedText = (textBox.Text).Split(' ');
            return splittedText[0];
        }







    }
}