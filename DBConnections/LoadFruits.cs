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
    internal class LoadFruits
    {
        MySqlConnection connection = new MySqlConnection();

        public List<Fruits> GetFruits(String id_supplier)
        {
            List<Fruits> fruits = new List<Fruits>();

            try
            {
                ConnectionStringSettings conString;
                conString = ConfigurationManager.ConnectionStrings["connectionString"];
                connection.ConnectionString = conString.ConnectionString;
                connection.Open();

                string query = "SELECT * FROM fruits WHERE idSupplier = @ID";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id_supplier);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Fruits fruit = new Fruits
                    {
                        Id = reader.GetInt32("idfruits"),
                        IdSupplier = reader.GetInt32("idSupplier"),
                        Fruit_Name = reader.GetString("fruit_name"),
                        Price = reader.GetInt32("price")
                    };
                    fruits.Add(fruit);
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

            return fruits;
        }

    }
}
