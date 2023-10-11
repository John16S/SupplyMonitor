using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyMonitor
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void RecalculateTotalPrice()
        {
            // Получаем текст из priceTextBox и Разделяем строку по пробелу, то есть из "120 р/кг" берем "120"
            string[] priceText = (priceTextBox.Text).Split(' ');
            
            if (priceText.Length > 0 && int.TryParse(priceText[0], out int price) && decimal.TryParse(amountNumericUpDown.Value.ToString(), out decimal amount))
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
            TextBox textBox = (TextBox)sender;

            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            string text = textBox.Text;

            // Удаляем все нечисловые символы из текста
            text = new string(text.Where(char.IsDigit).ToArray());

            // Если текст не пустой, добавляем " р/кг" к числу
            if (!string.IsNullOrWhiteSpace(text))
            {
                text += " р/кг";
            }

            textBox.Text = text;

            // Восстанавливаем положение курсора
            textBox.SelectionStart = selectionStart + (text.Length - textBox.Text.Length);
            textBox.SelectionLength = selectionLength;

            RecalculateTotalPrice();
        }


        private void amountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            RecalculateTotalPrice();
        }

        private void totalPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }


        private void priceTextBox_Leave(object sender, EventArgs e)
        {
            
        }
    }
}