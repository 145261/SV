using System;
using System.Data.SqlClient;

namespace LibraryDBManager
{
    class Program
    {
        static string connectionString = "Data Source=bd-kip.fa.ru\\sqlexpress;Initial Catalog=practucaSV20;Integrated Security=false;";

        static void Main(string[] args)
        {
            Console.WriteLine("Укажите данные для входа:");
            var loginResult = ChangeSQLCredentials();
            Console.Clear();
            if (loginResult)
            {
                while (true)
                {
                    Console.WriteLine("Library Database Management");
                    Console.WriteLine("1. Display Data");
                    Console.WriteLine("2. Add Data");
                    Console.WriteLine("3. Update Data");
                    Console.WriteLine("4. Delete Data");
                    Console.WriteLine("5. Change SQL Credentials");
                    Console.WriteLine("6. Change Connection String");
                    Console.WriteLine("7. Exit");

                    Console.Write("Choose an option: ");
                    int choice;
                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            DisplayData();
                            break;
                        case 2:
                            AddData();
                            break;
                        case 3:
                            UpdateData();
                            break;
                        case 4:
                            DeleteData();
                            break;
                        case 5:
                            ChangeSQLCredentials();
                            break;
                        case 6:
                            ChangeConnectionString();
                            break;
                        case 7:
                            return;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
        }

        static void DisplayData()
        {
            Console.Clear();
            Console.WriteLine("Display Data");
            Console.WriteLine("1. Customers");
            Console.WriteLine("2. Order_details");
            Console.WriteLine("3. Orders");
            Console.WriteLine("4. Products");
            Console.Write("Choose a table: ");
            int tableChoice;
            if (!int.TryParse(Console.ReadLine(), out tableChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            string tableName = GetTableName(tableChoice);
            if (tableName == null) return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM {tableName}";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetValue(i) + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }

        static void AddData()
        {
            Console.Clear();
            Console.WriteLine("Add Data");
            Console.WriteLine("1. Customers");
            Console.WriteLine("2. Order_details");
            Console.WriteLine("3. Orders");
            Console.WriteLine("4. Products");
            Console.Write("Choose a table: ");
            int tableChoice;
            if (!int.TryParse(Console.ReadLine(), out tableChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            string tableName = GetTableName(tableChoice);
            if (tableName == null) return;

            Console.Write("Enter values (comma-separated): ");
            string values = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO {tableName} VALUES ({values})";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Data added successfully.");
            }
        }

        static void UpdateData()
        {
            Console.Clear();
            Console.WriteLine("Update Data");
            Console.WriteLine("1. Customers");
            Console.WriteLine("2. Order_details");
            Console.WriteLine("3. Orders");
            Console.WriteLine("4. Products");
            Console.Write("Choose a table: ");
            int tableChoice;
            if (!int.TryParse(Console.ReadLine(), out tableChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            string tableName = GetTableName(tableChoice);
            if (tableName == null) return;

            Console.Write("Enter ID of the row to update: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            Console.Write("Enter column name to update: ");
            string columnName = Console.ReadLine();

            Console.Write("Enter new value: ");
            string newValue = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"UPDATE {tableName} SET {columnName} = @newValue WHERE {GetPrimaryKeyName(tableName)} = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@newValue", newValue);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Data updated successfully.");
            }
        }

        static void DeleteData()
        {
            Console.Clear();
            Console.WriteLine("Delete Data");
            Console.WriteLine("1. Customers");
            Console.WriteLine("2. Order_details");
            Console.WriteLine("3. Orders");
            Console.WriteLine("4. Products");
            Console.Write("Choose a table: ");
            int tableChoice;
            if (!int.TryParse(Console.ReadLine(), out tableChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            string tableName = GetTableName(tableChoice);
            if (tableName == null) return;

            Console.Write("Enter ID of the row to delete: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"DELETE FROM {tableName} WHERE {GetPrimaryKeyName(tableName)} = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Data deleted successfully.");
            }
        }

        static bool ChangeSQLCredentials()
        {
            Console.Write("Enter new SQL Server username: ");
            string username = Console.ReadLine();

            Console.Write("Enter new SQL Server password: ");
            string password = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var builder = new SqlConnectionStringBuilder(connectionString)
                {
                    UserID = username,
                    Password = password
                };
                connectionString = builder.ToString();
                Console.WriteLine("SQL credentials updated successfully.");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid credentials.");
                return false;
            }
        }

        static void ChangeConnectionString()
        {
            Console.Clear();
            Console.Write("Enter new connection string: ");
            connectionString = Console.ReadLine();
            Console.WriteLine("Connection string updated successfully.");
        }

        static string GetTableName(int choice)
        {
            switch (choice)
            {
                case 1:
                    return "Customers";
                case 2:
                    return "Order_details";
                case 3:
                    return "Orders";
                case 4:
                    return "Products";
                default:
                    Console.WriteLine("Invalid choice.");
                    return null;
            }
        }

        static string GetPrimaryKeyName(string tableName)
        {
            switch (tableName)
            {
                case "Customers":
                    return "customer_id";
                case "Order_details":
                    return "order_id";
                case "Orders":
                    return "order_id";
                case "Products":
                    return "product_id";
                default:
                    throw new ArgumentException("Invalid table name");
            }
        }
    }
}
