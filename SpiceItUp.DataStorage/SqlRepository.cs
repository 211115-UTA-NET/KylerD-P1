using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class SqlRepository
    {
        private static string? connectionString;

        private static List<int> customerIDList = new List<int>();

        private static List<string> transList = new List<string>();

        public SqlRepository(string CS)
        {
            connectionString = CS;
        }

        public static IEnumerable<User> SearchCustomerFirstName(string firstName)
        {
            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

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
            }

            return result;
        }

        public static IEnumerable<User> SearchCustomerLastName(string lastName)
        {
            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

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
            }

            return result;
        }

        public static IEnumerable<User> CustomerList()
        {
            customerIDList.Clear();

            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

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

                entry++;
            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Transaction> CustomerTransactionHistory(int customerID)
        {
            List<Transaction> result = new List<Transaction>();

            transList.Clear();

            using SqlConnection connection = new(connectionString);

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
                entry++;
            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Transaction> DetailedTransaction(string id)
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
            }
            connection.Close();

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
            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Store> PrintStoreList()
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
            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Store> PrintStoreInventory(int storeEntry)
        {
            List<Store> result = new List<Store>();

            using SqlConnection connection = new(connectionString);

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
            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Transaction> StoreTransactionHistory(int storeID)
        {
            List<Transaction> result = new List<Transaction>();

            transList.Clear();

            using SqlConnection connection = new(connectionString);

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
                entry++;
            }
            connection.Close();

            return result;
        }

        public static IEnumerable<User> GetLoginUserID(string username, string password)
        {
            List<User> result = new List<User>();

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
                int userID = loginReader.GetInt32(0);
                result.Add(new(userID));
            }
            connection.Close();

            return result;
        }

        public static IEnumerable<User> GetCustomerInfo(int id)
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

        public static void PostCustomerInfo(string newUsername, string newPassword, string firstName, string lastName, double phoneNumber)
        {
            using SqlConnection connection = new(connectionString);

            // Add the customer's login information to SQL
            connection.Open();
            string addNewLoginManager = $"INSERT LoginManager (Username, \"Password\") VALUES (@username, @password);";
            using SqlCommand newLoginManagerCommand = new(addNewLoginManager, connection);
            newLoginManagerCommand.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = newUsername;
            newLoginManagerCommand.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = newPassword;
            newLoginManagerCommand.ExecuteNonQuery();
            connection.Close();

            // Extract the new ID number that was automatically created
            connection.Open();
            string getNewUserID = $"SELECT UserID FROM LoginManager WHERE Username = @username;";
            using SqlCommand readNewUserID = new(getNewUserID, connection);
            readNewUserID.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = newUsername;
            using SqlDataReader reader = readNewUserID.ExecuteReader();
            int finalIDGrab = 0;
            while (reader.Read())
            {
                finalIDGrab = reader.GetInt32(0);
            }
            connection.Close();

            // Add the customer's personal information to SQL
            connection.Open();
            string addNewCustomer = $"INSERT UserInformation (UserID, FirstName, LastName, PhoneNumber, IsEmployee) " +
                $"VALUES (@customerID, @firstName, @lastName, @phoneNumber, @isEmployee);";
            using SqlCommand newUserCreationCommand = new(addNewCustomer, connection);
            newUserCreationCommand.Parameters.Add("@customerID", System.Data.SqlDbType.Int).Value = finalIDGrab;
            newUserCreationCommand.Parameters.Add("@firstName", System.Data.SqlDbType.VarChar).Value = firstName;
            newUserCreationCommand.Parameters.Add("@lastName", System.Data.SqlDbType.VarChar).Value = lastName;
            newUserCreationCommand.Parameters.Add("@phoneNumber", System.Data.SqlDbType.BigInt).Value = phoneNumber;
            newUserCreationCommand.Parameters.Add("@isEmployee", System.Data.SqlDbType.VarChar).Value = "FALSE";
            newUserCreationCommand.ExecuteNonQuery();
            connection.Close();
        }

        public static IEnumerable<Store> GetStoreInfo(int storeEntry)
        {
            List<Store> result = new List<Store>();

            using SqlConnection connection = new(connectionString);

            //Pull information from the entered store (Name)
            connection.Open();
            string getSelectedStore = $"SELECT * FROM StoreInfo WHERE StoreID = @storeID;";
            using SqlCommand readSelectedStore = new(getSelectedStore, connection);
            readSelectedStore.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readStore = readSelectedStore.ExecuteReader();
            while (readStore.Read())
            {
                int storeID = readStore.GetInt32(0);
                string storeName = readStore.GetString(1);
                result.Add(new(storeID, storeName));
            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Store> GetCartStoreInventory(int storeEntry)
        {
            List<Store> result = new List<Store>();

            using SqlConnection connection = new(connectionString);

            //Begin pulling the inventory from the selected store
            connection.Open();
            string getStoreInventory = "SELECT ItemDetails.ItemID, ItemDetails.ItemName, StoreInventory.InStock, ItemDetails.ItemPrice " +
                "FROM StoreInventory JOIN ItemDetails " +
                "ON StoreInventory.ItemID = ItemDetails.ItemID " +
                "WHERE StoreInventory.StoreID = @storeID ORDER BY ItemDetails.ItemID;";
            using SqlCommand readStoreInventory = new(getStoreInventory, connection);
            readStoreInventory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readInventory = readStoreInventory.ExecuteReader();
            while (readInventory.Read())
            {
                int id = readInventory.GetInt32(0);
                string name = readInventory.GetString(1);
                int quantity = readInventory.GetInt32(2);
                decimal price = readInventory.GetDecimal(3);
                //Inventory is stored
                result.Add(new(id, name, quantity, price));
            }
            connection.Close();

            return result;
        }

        public static void NewTransaction(string transID, int userID, int storeEntry)
        {
            using SqlConnection connection = new(connectionString);

            connection.Open();
            string addTransHistory = "INSERT TransactionHistory (TransactionID, UserID, StoreID, IsStoreOrder, Timestamp) " +
                "VALUES (@transID, @userID, @storeID, @isStoreOrder, @timestamp);";
            using SqlCommand newTransHistory = new(addTransHistory, connection);
            DateTime now = DateTime.Now;
            string dateTime = now.ToString("F");
            newTransHistory.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = transID;
            newTransHistory.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = userID;
            newTransHistory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            newTransHistory.Parameters.Add("@isStoreOrder", System.Data.SqlDbType.VarChar).Value = "FALSE";
            newTransHistory.Parameters.Add("@timestamp", System.Data.SqlDbType.NVarChar).Value = dateTime;
            newTransHistory.ExecuteNonQuery();
            connection.Close();
        }

        public static void TransactionDetails(string transID, int customerItemIDNew, int customerQuantityNew, decimal customerPriceNew)
        {
            using SqlConnection connection = new(connectionString);

            //Add customer cart items to database
            connection.Open();
            string addTransDetails = "INSERT CustomerTransactionDetails (TransactionID, ItemID, Quantity, Price) " +
                "VALUES (@transID, @itemID, @quantity, @price);";
            using SqlCommand newTransDetails = new(addTransDetails, connection);
            newTransDetails.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = transID;
            newTransDetails.Parameters.Add("@itemID", System.Data.SqlDbType.Int).Value = customerItemIDNew;
            newTransDetails.Parameters.Add("@quantity", System.Data.SqlDbType.Int).Value = customerQuantityNew;
            newTransDetails.Parameters.Add("@price", System.Data.SqlDbType.Money).Value = customerPriceNew;
            newTransDetails.ExecuteNonQuery();
            connection.Close();
        }

        public static void NewStoreInventory(int inStockListNew, int storeEntry, int itemIDListNew)
        {
            using SqlConnection connection = new(connectionString);

            //Updates store quantites based on what customer has added to their cart
            connection.Open();
            string updateStoreInv = "UPDATE StoreInventory SET InStock = @stock WHERE StoreID = @storeID AND ItemID = @itemID;";
            using SqlCommand newStoreInv = new(updateStoreInv, connection);
            newStoreInv.Parameters.Add("@stock", System.Data.SqlDbType.Int).Value = inStockListNew;
            newStoreInv.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            newStoreInv.Parameters.Add("@itemID", System.Data.SqlDbType.Int).Value = itemIDListNew;
            newStoreInv.ExecuteNonQuery();
            connection.Close();
        }
    }
}