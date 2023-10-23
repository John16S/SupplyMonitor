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

namespace SupplyMonitor.Views
{
    public partial class ReportPage : Form
    {
        Form form;
        public ReportPage(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string startDate = dateTimeFrom.Value.ToString("yyyy-MM-dd");
            string endDate = dateTimeTo.Value.ToString("yyyy-MM-dd");

            GetOrders getOrders = new GetOrders();
            List<Orders> orders = getOrders.getOrders(startDate, endDate);

            int totalPriceInRange = 0;

            // Очистите существующие строки в dataGridView
            dataGridView1.Rows.Clear();

            // Добавьте каждый заказ в dataGridView
            foreach (Orders order in orders)
            {
                totalPriceInRange += order.totalPrice;
                dataGridView1.Rows.Add(
                    order.fruitName,
                    order.supplierName,
                    order.weight,
                    order.totalPrice,
                    order.dateTime.ToShortDateString()
                );
            }
            label5.Text = totalPriceInRange.ToString() + " руб.";
        }

        private void ReportPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            form.Show();
        }

        private void dateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimeFrom.Value > dateTimeTo.Value)
                dateTimeTo.Value = dateTimeFrom.Value;
        }

        private void dateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimeTo.Value < dateTimeFrom.Value)
                dateTimeTo.Value = dateTimeFrom.Value;
        }
    }
}
