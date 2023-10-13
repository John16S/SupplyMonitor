using MySql.Data.MySqlClient;
using SupplyMonitor.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyMonitor.DBConnections
{
    internal class GetOrders
    {
        MySqlConnection connection = new MySqlConnection();

        public List<Orders> getOrders(string StartDate, string EndDate)
        {
            List<Orders> orders = new List<Orders>();

            try
            {
                ConnectionStringSettings conString;
                conString = ConfigurationManager.ConnectionStrings["connectionString"];
                connection.ConnectionString = conString.ConnectionString;
                connection.Open();

                string getQuery = @"Select f.fruit_name, 
	                                       s.name,
                                           o.amount,
                                           o.totalPrice,
                                           o.orderDate
                                    From orders o inner join fruits f 
                                    on o.fruit_id = f.idfruits
                                    inner join supplier s 
                                    on f.idSupplier = s.idSupplier
                                    WHERE o.orderDate >= @StartDate AND o.orderDate <= @EndDate
                                    order by s.name";

                MySqlCommand command = new MySqlCommand(getQuery, connection);
                command.Parameters.AddWithValue("@StartDate", StartDate);
                command.Parameters.AddWithValue("@EndDate", EndDate);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Orders order = new Orders()
                    {
                        fruitName = reader.GetString("fruit_name"),
                        supplierName = reader.GetString("name"),
                        weight = reader.GetInt32("amount"),
                        totalPrice = reader.GetInt32("totalPrice"),
                        dateTime = reader.GetDateTime("orderDate")
                    };
                    orders.Add(order);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Обработка ошибки, например, вывод сообщения об ошибке или запись в журнал
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return orders;
        }
    }
}
