using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class IRepository
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        private static List<int> customerIDList = new List<int>();

        private static List<string> transList = new List<string>();

        public IEnumerable<User> SearchCustomerFirstName(string firstName)
        {
            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "=======", "=========", "=========", "============", "============"));

            //Pull customer information matching first name entered
            connection.Open();
            string customerSearch = "SELECT UserID, FirstName, LastName, PhoneNumber, IsEmployee FROM UserInformation " +
                "WHERE FirstName = @firstName ORDER BY LastName;";
            using SqlCommand getCustomer = new(customerSearch, connection);
            getCustomer.Parameters.Add("@firstName", System.Data.SqlDbType.VarChar).Value = firstName;
            using SqlDataReader reader = getCustomer.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string first = reader.GetString(1);
                string last = reader.GetString(2);
                double phone = reader.GetInt64(3);
                string employee = reader.GetString(4);
                result.Add(new(id, first, last, phone, employee));

                Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3), reader.GetString(4)));
            }
            Console.WriteLine("==============================");

            return result;
        }

        public IEnumerable<User> SearchCustomerLastName(string lastName)
        {
            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "=======", "=========", "=========", "============", "============"));

            //Pull customer information matching last name entered
            connection.Open();
            string customerSearch = "SELECT UserID, FirstName, LastName, PhoneNumber, IsEmployee FROM UserInformation " +
                "WHERE LastName = @lastName ORDER BY LastName;";
            using SqlCommand getCustomer = new(customerSearch, connection);
            getCustomer.Parameters.Add("@lastName", System.Data.SqlDbType.VarChar).Value = lastName;
            using SqlDataReader reader = getCustomer.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string first = reader.GetString(1);
                string last = reader.GetString(2);
                double phone = reader.GetInt64(3);
                string employee = reader.GetString(4);
                result.Add(new(id, first, last, phone, employee));

                Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                       reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3), reader.GetString(4)));
            }
            Console.WriteLine("==============================");

            return result;
        }

        public IEnumerable<User> CustomerList()
        {
            customerIDList.Clear();

            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

            //Format our customer list
            Console.WriteLine("Here is the customer list:");
            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    "Entry", "First Name", "Last Name", "Phone Number"));
            Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    "=====", "==========", "=========", "============"));

            //Pull a list of customers from the database
            connection.Open();
            string getCustomerList = "SELECT UserID, FirstName, LastName, PhoneNumber FROM UserInformation WHERE IsEmployee = 'FALSE';";
            using SqlCommand readCustomerList = new(getCustomerList, connection);
            using SqlDataReader readCustomers = readCustomerList.ExecuteReader();
            int entry = 1;
            while (readCustomers.Read())
            {
                int id = readCustomers.GetInt32(0);
                string first = readCustomers.GetString(1);
                string last = readCustomers.GetString(2);
                double phone = readCustomers.GetInt64(3);
                result.Add(new(id, first, last, phone));

                customerIDList.Add(readCustomers.GetInt32(0));
                Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    entry, readCustomers.GetString(1), readCustomers.GetString(2), readCustomers.GetInt64(3)));
                entry++;
            }
            connection.Close();

            return result;
        }

        public IEnumerable<Transaction> CustomerTransactionHistory(int customerID)
        {
            List<Transaction> result = new List<Transaction>();

            transList.Clear();

            using SqlConnection connection = new(connectionString);
            //Format our transaction list
            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                    "Entry", "Transaction ID", "Store ID", "Total Price"));
            Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                    "=====", "==============", "========", "==========="));

            //Get transaction list from database and print
            connection.Open();
            string getOrderHistory = "SELECT TransactionHistory.TransactionID, TransactionHistory.StoreID, SUM(CustomerTransactionDetails.Price) " +
                "FROM TransactionHistory JOIN CustomerTransactionDetails " +
                "ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID " +
                "WHERE TransactionHistory.UserID = @userID GROUP BY TransactionHistory.TransactionID, TransactionHistory.StoreID;";
            using SqlCommand orderHistory = new(getOrderHistory, connection);
            orderHistory.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = customerID;
            using SqlDataReader reader = orderHistory.ExecuteReader();
            int entry = 1;
            while (reader.Read())
            {
                string transID = reader.GetString(0);
                int storeID = reader.GetInt32(1);
                string price = String.Format("{0:0.00}", reader.GetDecimal(2));
                result.Add(new(transID, storeID, price));

                transList.Add(reader.GetString(0));
                //string price = String.Format("{0:0.00}", reader.GetDecimal(2));
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                    entry, reader.GetString(0), reader.GetInt32(1), $"${price}"));
                entry++;
            }
            connection.Close();

            return result;
        }

        public IEnumerable<Transaction> DetailedTransaction(string id)
        {
            List<Transaction> result = new List<Transaction>();

            using SqlConnection connection = new(connectionString);

            //Pull transaction details from database and print
            connection.Open();
            string getOpening = "SELECT TransactionHistory.TransactionID, StoreInfo.StoreID, StoreInfo.StoreName, " +
                "TransactionHistory.Timestamp, UserInformation.FirstName, UserInformation.LastName, SUM(CustomerTransactionDetails.Price) " +
                "FROM TransactionHistory JOIN StoreInfo " +
                "ON TransactionHistory.StoreID = StoreInfo.StoreID " +
                "JOIN CustomerTransactionDetails " +
                "ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID " +
                "JOIN UserInformation " +
                "ON Userinformation.UserID = TransactionHistory.UserID " +
                "WHERE TransactionHistory.TransactionID = @transID " +
                "GROUP BY TransactionHistory.TransactionID, StoreInfo.StoreID, StoreInfo.StoreName, TransactionHistory.Timestamp, UserInformation.FirstName, UserInformation.LastName;";
            using SqlCommand readOpening = new(getOpening, connection);
            readOpening.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = id;
            using SqlDataReader myReader = readOpening.ExecuteReader();

            string transID = "";
            int storeID = 0;
            string storeName = "";
            string timestamp = "";
            string firstName = "";
            string lastName = "";
            string totalPrice = "";

            while (myReader.Read())
            {
                transID = myReader.GetString(0);
                storeID = myReader.GetInt32(1);
                storeName = myReader.GetString(2);
                timestamp = myReader.GetString(3);
                firstName = myReader.GetString(4);
                lastName = myReader.GetString(5);
                totalPrice = String.Format("{0:0.00}", myReader.GetDecimal(6));

                Console.WriteLine("==============================");
                Console.WriteLine($"Transaction ID: {myReader.GetString(0)}");
                Console.WriteLine($"Store {myReader.GetInt32(1)}: {myReader.GetString(2)}");
                Console.WriteLine($"Name: {myReader.GetString(4)} {myReader.GetString(5)}");
                Console.WriteLine($"Time: {myReader.GetString(3)}");
                Console.WriteLine($"Total: ${totalPrice}");
                Console.WriteLine("==============================");
            }
            connection.Close();

            //Format transaction information
            Console.WriteLine("Item Name\t Quantity\t Price");
            Console.WriteLine("=========\t ========\t =====");

            //Print items that were bought in transaction
            connection.Open();
            string getDetails = "SELECT ItemDetails.ItemName, CustomerTransactionDetails.Quantity, CustomerTransactionDetails.Price " +
                "FROM CustomerTransactionDetails JOIN ItemDetails ON CustomerTransactionDetails.ItemID = ItemDetails.ItemID " +
                "WHERE CustomerTransactionDetails.TransactionID = @transID;";
            using SqlCommand readDetails = new(getDetails, connection);
            readDetails.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = id;
            using SqlDataReader detailReader = readDetails.ExecuteReader();
            while (detailReader.Read())
            {
                string itemName = detailReader.GetString(0);
                int quantity = detailReader.GetInt32(1);
                string itemPrice = String.Format("{0:0.00}", detailReader.GetDecimal(2));
                result.Add(new(transID, storeID, storeName, timestamp, firstName, lastName, totalPrice, itemName, quantity, itemPrice));

                Console.WriteLine(String.Format("{0, -16} {1, -16} {2, -16}",
                detailReader.GetString(0), detailReader.GetInt32(1), $"${itemPrice}"));
            }
            Console.WriteLine("==============================");
            connection.Close();

            return result;
        }

        public IEnumerable<Store> PrintStoreList()
        {
            List<Store> result = new List<Store>();

            using SqlConnection connection = new(connectionString);

            //Get list of stores from database
            connection.Open();
            string getStoreInfo = "SELECT * FROM StoreInfo ORDER BY StoreID;";
            using SqlCommand readStoreInfo = new(getStoreInfo, connection);
            using SqlDataReader reader = readStoreInfo.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                result.Add(new(id, name));

                Console.WriteLine($"Store {reader.GetInt32(0)}: {reader.GetString(1)}");
            }
            connection.Close();

            return result;
        }

        public IEnumerable<Store> PrintStoreInventory(int storeEntry)
        {
            List<Store> result = new List<Store>();

            using SqlConnection connection = new(connectionString);

            //Pull the selected store information
            connection.Open();
            string getSelectedStore = $"SELECT * FROM StoreInfo WHERE StoreID = @storeID;";
            using SqlCommand readSelectedStore = new(getSelectedStore, connection);
            readSelectedStore.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readStore = readSelectedStore.ExecuteReader();
            while (readStore.Read())
            {
                Console.WriteLine($"Inventory for store {readStore.GetInt32(0)}: {readStore.GetString(1)}");
            }
            connection.Close();

            //Formatting
            Console.WriteLine("=================================");
            Console.WriteLine("Item Name\t In Stock\t Price");
            Console.WriteLine("=========\t ========\t =====");

            //Pull and print the store's inventory
            connection.Open();
            string getStoreInventory = "SELECT ItemDetails.ItemName, StoreInventory.InStock, ItemDetails.ItemPrice " +
                "FROM StoreInventory JOIN ItemDetails " +
                "ON StoreInventory.ItemID = ItemDetails.ItemID " +
                "WHERE StoreInventory.StoreID = @storeID ORDER BY ItemDetails.ItemName;";
            using SqlCommand readStoreInventory = new(getStoreInventory, connection);
            readStoreInventory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readInventory = readStoreInventory.ExecuteReader();
            while (readInventory.Read())
            {
                string name = readInventory.GetString(0);
                int quantity = readInventory.GetInt32(1);
                decimal itemPrice = readInventory.GetDecimal(2);
                string price = String.Format("{0:0.00}", itemPrice);
                result.Add(new(name, quantity, price));

                Console.WriteLine(String.Format("{0, -16} {1, -15} {2, -16}", readInventory.GetString(0), readInventory.GetInt32(1), $"${price}"));
            }
            connection.Close();

            return result;
        }

        public IEnumerable<Transaction> StoreTransactionHistory(int storeID)
        {
            List<Transaction> result = new List<Transaction>();

            transList.Clear();

            using SqlConnection connection = new(connectionString);

            //Format our transactions
            Console.WriteLine($"Order history for store {storeID}");
            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                    "Entry", "Transaction ID", "First Name", "Last Name", "Total Price"));
            Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                    "=====", "==============", "==========", "==========", "=========="));

            //Print off transactions at selected store from database
            connection.Open();
            string getOrderHistory = "SELECT TransactionHistory.TransactionID, UserInformation.FirstName, UserInformation.LastName, " +
                "SUM(CustomerTransactionDetails.Price) " +
                "FROM TransactionHistory JOIN UserInformation " +
                "ON TransactionHistory.UserID = UserInformation.UserID " +
                "JOIN CustomerTransactionDetails " +
                "ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID " +
                "WHERE TransactionHistory.StoreID = @storeID " +
                "GROUP BY TransactionHistory.TransactionID, UserInformation.FirstName, UserInformation.LastName;";
            using SqlCommand storeOrderHistory = new(getOrderHistory, connection);
            storeOrderHistory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeID;
            using SqlDataReader reader = storeOrderHistory.ExecuteReader();
            int entry = 1;
            while (reader.Read())
            {
                string id = reader.GetString(0);
                string first = reader.GetString(1);
                string last = reader.GetString(2);
                string price = String.Format("{0:0.00}", reader.GetDecimal(3));
                result.Add(new(id, first, last, price));

                transList.Add(reader.GetString(0));
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                    entry, reader.GetString(0), reader.GetString(1), reader.GetString(2), $"${price}"));
                entry++;
            }
            connection.Close();

            return result;
        }

        public IEnumerable<User> GetLoginUserID(string username, string password)
        {
            List<User> result = new List<User>();

            int userID = 0;

            using SqlConnection connection = new(connectionString);

            //If username and password is a valid entry, pull a UserID
            connection.Open();
            string getLoginManager = $"SELECT UserID FROM LoginManager WHERE (Username = @username AND \"Password\" = @password);";
            using SqlCommand readLoginManager = new(getLoginManager, connection);
            readLoginManager.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = username;
            readLoginManager.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = password;
            using SqlDataReader loginReader = readLoginManager.ExecuteReader();
            while (loginReader.Read())
            {
                userID = loginReader.GetInt32(0);
                result.Add(new(userID));
            }
            connection.Close();

            return result;
        }

        public IEnumerable<User> GetCustomerInfo(int id)
        {
            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

            connection.Open();
            string getUserInfo = $"SELECT * FROM UserInformation WHERE UserID = @validUserID;";
            using SqlCommand readUserInfo = new(getUserInfo, connection);
            readUserInfo.Parameters.Add("@validUserID", System.Data.SqlDbType.Int).Value = id;
            using SqlDataReader userReader = readUserInfo.ExecuteReader();
            while (userReader.Read())
            {
                int userID = userReader.GetInt32(0);
                string first = userReader.GetString(1);
                string last = userReader.GetString(2);
                double phone = userReader.GetInt64(3);
                string employee = userReader.GetString(4);
                result.Add(new(userID, first, last, phone, employee));
            }
            connection.Close();

            return result;
        }
    }
}