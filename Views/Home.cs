using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using SupplyMonitor.DBConnections;
using SupplyMonitor.Models;
using SupplyMonitor.Views;
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

        List<Cart> cartList = new List<Cart>();

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

        /// <summary>
        /// Считает общую сумму фрукта по весу
        /// </summary>
        private void RecalculateTotalPrice()
        {
            // Получаем текст из priceTextBox и Разделяем строку по пробелу, то есть из "120 р/кг" берем "120"
            string priceText = CutTheUnnecesarryPart(priceTextBox);
            
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

        /// <summary>
        /// Обработчик при изменение цены фрукта
        /// </summary>
        private void priceTextBox_TextChanged(object sender, EventArgs e)
        {
            RecalculateTotalPrice();
        }

        /// <summary>
        /// Обработчик при изменение веса
        /// </summary>
        private void amountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            RecalculateTotalPrice();
        }

        /// <summary>
        /// Обработчик при выобре поставщика
        /// </summary>
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

        /// <summary>
        /// Обработчик при выборе типа фрукты
        /// </summary>
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

        /// <summary>
        /// Кнопка добавление в Корзину
        /// </summary>
        private void addToCartBtn_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            try
            {
                cart.Suplplier = supplierBox.Text.ToString();
                cart.IdSupplier = int.TryParse((supplierBox.SelectedValue.ToString()), out int idsupplier) ? idsupplier : 0;
                cart.Fruit = typeOfFriutBox.Text;
                cart.IdFruit = int.TryParse((typeOfFriutBox.SelectedValue.ToString()), out int idfruit) ? idfruit : 0;
                cart.Price = int.TryParse(CutTheUnnecesarryPart(priceTextBox), out int price) ? price : 0;
                cart.Weight = (amountNumericUpDown.Value != 0) ? (int)amountNumericUpDown.Value : throw new Exception("Вес не должен быть равен 0!");
                cart.TotalPrice = int.TryParse(CutTheUnnecesarryPart(totalPriceTextBox), out int totalPrice) ? totalPrice : 0;
                cartList.Add(cart);
                dataGridView1.Rows.Add(cart.Suplplier, cart.Fruit, cart.Price, cart.Weight, cart.TotalPrice);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            //cart.print();
        }

        /// <summary>
        /// Обрезает всю не нужную часть
        /// </summary>
        private string CutTheUnnecesarryPart(TextBox textBox)
        {
            string[] splittedText = (textBox.Text).Split(' ');
            return splittedText[0];
        }

        /// <summary>
        /// Нажатие кнопки заказать, вненсение данных в БД
        /// </summary>
        private void orderBtn_Click(object sender, EventArgs e)
        {
            InsertFromCartToOrdersHistory insert = new InsertFromCartToOrdersHistory();
            Task<bool> isInserted = Task.Run(() => insert.insert(cartList));
            isInserted.ContinueWith(task =>
            {
                if (task.Result)
                {
                    MessageBox.Show("Товар успешн внесен в БД!");
                    dataGridView1.Rows.Clear();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Удаление элементов из корзины (и из списка cartList)
        /// </summary>
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                // Проверяем, есть ли выбранная строка
                foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                {
                    // Получаем значения из выбранной строки в DataGridView
                    string supplier = selectedRow.Cells["Supplier"].Value.ToString();
                    string fruit = selectedRow.Cells["FruitType"].Value.ToString();
                    int price = Convert.ToInt32(selectedRow.Cells["Price"].Value);
                    int weight = Convert.ToInt32(selectedRow.Cells["Amount"].Value);
                    int totalPrice = Convert.ToInt32(selectedRow.Cells["TotalPrice"].Value);

                    // Находим соответствующий объект Cart в cartList
                    Cart removedCart = cartList.FirstOrDefault(cart =>
                         cart.Suplplier == supplier && cart.Fruit == fruit && cart.Price == price
                         && cart.Weight == weight && cart.TotalPrice == totalPrice);


                    if (removedCart != null)
                    {
                        // Удаляем соответствующий объект Cart из cartList
                        cartList.Remove(removedCart);
                    }

                    // Удаляем выбранную строку
                    dataGridView1.Rows.Remove(selectedRow);
                }
            }
        }

        /// <summary>
        /// Кнопка отчета
        /// </summary>
        private void reportBtn_Click(object sender, EventArgs e)
        {
            ReportPage reportPage = new ReportPage(this);
            Hide();
            reportPage.Show();
        }

        /// <summary>
        /// Закрытие приложения
        /// </summary>
        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}