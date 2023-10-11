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
    internal class LoadSuppliers
    {
        MySqlConnection connection = new MySqlConnection();

        public List<Supplier> GetSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            try
            {
                ConnectionStringSettings conString;
                conString = ConfigurationManager.ConnectionStrings["connectionString"];
                connection.ConnectionString = conString.ConnectionString;
                connection.Open();

                string query = "SELECT * FROM supplier";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Supplier supplier = new Supplier
                    {
                        Id = reader.GetInt32("idSupplier"),
                        Name = reader.GetString("name")
                    };
                    suppliers.Add(supplier);
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

            return suppliers;
        }

    }
}
