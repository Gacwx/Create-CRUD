using System;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace ADO.NETconnection
{
    public class Task1
    {
        public static void Main()
        {
            string connectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))\r\n(CONNECT_DATA=(SERVER=DEDICATED)(sid=orcl)));User Id=gachay;\r\nPassword=Qmeafq16;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                // Open the Connection
                connection.Open();

                // Create Table
                string createTableCommandText = @"
                    BEGIN
                        EXECUTE IMMEDIATE 'CREATE TABLE Product (
                            ID NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                            ProductName NVARCHAR2(255),
                            Quantity NUMBER,
                            Price NUMBER(10,2))';
                    EXCEPTION WHEN OTHERS THEN
                        IF SQLCODE != -955 THEN RAISE; END IF;
                    END;";

                using (OracleCommand command = new OracleCommand(createTableCommandText, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Product' Created Successfully.");
                }

                // Insert Data
                using (OracleCommand insertCmd = new OracleCommand("INSERT INTO Product (ProductName, Quantity, Price) VALUES ('Laptop', 10, 799.99)", connection))
                {
                    insertCmd.ExecuteNonQuery();
                }
                using (OracleCommand insertCmd = new OracleCommand("INSERT INTO Product (ProductName, Quantity, Price) VALUES ('Mouse', 100, 19.99)", connection))
                {
                    insertCmd.ExecuteNonQuery();
                }

                // Read Data
                Console.WriteLine("Initial Data:");
                using (OracleCommand readCmd = new OracleCommand("SELECT * FROM Product", connection))
                using (OracleDataReader reader = readCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ID"]}, Name: {reader["ProductName"]}, Quantity: {reader["Quantity"]}, Price: {reader["Price"]}");
                    }
                }

                // Update Data
                using (OracleCommand updateCmd = new OracleCommand("UPDATE Product SET ProductName = 'Updated Product', Quantity = 50, Price = 20.99 WHERE ID = 1", connection))
                {
                    updateCmd.ExecuteNonQuery();
                }

                // Delete Data
                using (OracleCommand deleteCmd = new OracleCommand("DELETE FROM Product WHERE ID = 2", connection))
                {
                    deleteCmd.ExecuteNonQuery();
                }

                // Read Data Again
                Console.WriteLine("Updated Data:");
                using (OracleCommand readCmd = new OracleCommand("SELECT * FROM Product", connection))
                using (OracleDataReader reader = readCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ID"]}, Name: {reader["ProductName"]}, Quantity: {reader["Quantity"]}, Price: {reader["Price"]}");
                    }
                }

                connection.Close();
            }
        }
    }
}
