using MySql.Data.MySqlClient;
using SupplyMonitor.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyMonitor.DBConnections
{
    internal class InsertFromCartToOrdersHistory
    {
        bool inserted = false;
        MySqlConnection connection = new MySqlConnection();

        public bool insert(List<Cart> cartList)
        {
            try
            {
                ConnectionStringSettings conString;
                conString = ConfigurationManager.ConnectionStrings["connectionString"];
                connection.ConnectionString = conString.ConnectionString;
                connection.Open();

                for (int i = 0; i < cartList.Count; i++)
                {
                    Cart cartItem = cartList[i];

                    string insertQuerry = "INSERT INTO orders (fruit_id, amount, totalPrice, orderDate) " +
                        "VALUES (@fruit_id, @amount, @totalPrice, @orderDate)";
                    MySqlCommand command = new MySqlCommand(insertQuerry, connection);
                    command.Parameters.AddWithValue("@fruit_id", cartItem.IdFruit);
                    command.Parameters.AddWithValue("@amount", cartItem.Weight);
                    command.Parameters.AddWithValue("@totalPrice", cartItem.TotalPrice);
                    command.Parameters.AddWithValue("@orderDate", DateTime.Now.ToString("yyyy-MM-dd"));

                    int insertedRows = command.ExecuteNonQuery();

                    if (insertedRows > 0)
                    {
                        inserted = true;
                    }
                    else
                    {
                        throw new Exception("Ошибка вставки записи!");
                    }
                }

            }
            catch (Exception ex) 
            {
                inserted = false;
                // Обработка ошибки, например, вывод сообщения об ошибке или запись в журнал
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return inserted;
        }
    }
}
