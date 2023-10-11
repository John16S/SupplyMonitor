using MySql.Data.MySqlClient;
using SupplyMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyMonitor
{
    internal class DBConnection
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;user=root;password=root;database=supplymonitor");
        
        
        public MySqlConnection getConnection() { return connection; }

        public List<Supplier> GetSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            try
            {
                openConnection();
                string query = "SELECT * FROM supplymonitor.supplier";
                MySqlCommand command = new MySqlCommand(query, getConnection());
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
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            finally
            {
                closeConnection();
            }

            return suppliers;
        }

        public List<Fruits> GetFruits()
        {
            List<Fruits> fruits = new List<Fruits>();

            try
            {
                openConnection();
                string query = "SELECT * FROM supplymonitor.fruits";
                MySqlCommand command = new MySqlCommand(query, getConnection());
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
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            finally
            {
                closeConnection();
            }

            return fruits;
        }

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed) connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open) connection.Close();
        }


    }
}
