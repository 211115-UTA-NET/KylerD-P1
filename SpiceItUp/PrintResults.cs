using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class PrintResults
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        private static List<string> transList = new List<string>();

        private static List<int> customerIDList = new List<int>();

        /// <summary>
        /// Information from the database reguarding store inventory is stored in lists
        /// </summary>
        private static List<int> itemIDList = new List<int>();
        private static List<string> itemNameList = new List<string>();
        private static List<int> inStockList = new List<int>();
        private static List<decimal> priceList = new List<decimal>();

        /// <summary>
        /// Information reguarding what the customer has added or removed from their cart is stored in lists
        /// </summary>
        private static List<int> customerItemID = new List<int>();
        private static List<string> customerItemName = new List<string>();
        private static List<int> customerQuantity = new List<int>();
        private static List<decimal> customerPrice = new List<decimal>();

        /// <summary>
        /// The information entered by user is written to our database
        /// This information can be pulled and used to login to an account when entered in correctly
        /// </summary>
        public static void AddNewCustomer(string newUsername, string newPassword, string firstName, string lastName, string phoneNumber)
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

            Console.WriteLine($"Your account has been created, {firstName}! You may now login!");
        }

        /// <summary>
        /// Prints the basic summary of all store locations
        /// </summary>
        public static void PrintStoreList(IEnumerable<Store> stores)
        {
            //Get list of stores from database
            foreach (var record in stores)
            {
                Console.WriteLine($"Store {record.StoreID}: {record.StoreName}");
            }
        }

        /// <summary>
        /// We will attempt to pull the inventory from entered store
        /// Store inventory is pulled from database and formatted accordingly
        /// </summary>
        public static void PrintStoreInfo(IEnumerable<Store> inventory, int storeEntry)
        {
            Console.WriteLine($"Inventory for Store #{storeEntry}");
            //Formatting
            Console.WriteLine("=================================");
            Console.WriteLine("Item Name\t In Stock\t Price");
            Console.WriteLine("=========\t ========\t =====");

            //Pull and print the store's inventory
            foreach (var item in inventory)
            {
                string price = String.Format("{0:0.00}", item.ItemPrice);
                Console.WriteLine(String.Format("{0, -16} {1, -15} {2, -16}", item.ItemName, item.ItemQuantity, $"${price}"));
            }
        }

        /// <summary>
        /// Pulls the order history from the specified store
        /// A list of transaction IDs is returned
        /// </summary>
        /// <param name="myEntry"></param>
        /// <returns></returns>
        public static List<string> PrintStoreTransactionList(IEnumerable<Transaction> transactions, int myEntry)
        {
            transList.Clear();

            //Format our transactions
            Console.WriteLine($"Order history for store {myEntry}");
            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                    "Entry", "Transaction ID", "First Name", "Last Name", "Total Price"));
            Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                    "=====", "==============", "==========", "==========", "=========="));

            //Print off transactions at selected store from database
            int entry = 1;
            foreach(var record in transactions)
            {
                transList.Add(record.TransID);
                string price = String.Format("{0:0.00}", record.Price);
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                    entry, record.TransID, record.FirstName, record.LastName, $"${price}"));
                entry++;
            }

            return transList;
        }

        /// <summary>
        /// A transaction is printed off in more detail based on employee selection
        /// </summary>
        public static void DetailedTransaction(IEnumerable<Transaction> transaction)
        {
            foreach (var record in transaction)
            {
                Console.WriteLine("==============================");
                Console.WriteLine($"Transaction ID: {record.TransID}");
                Console.WriteLine($"Store {record.StoreID}: {record.StoreName}");
                Console.WriteLine($"Name: {record.FirstName} {record.LastName}");
                Console.WriteLine($"Time: {record.Timestamp}");
                Console.WriteLine($"Total: {record.TotalPrice}");
                Console.WriteLine("==============================");
                break;
            }

            //Format transaction information
            Console.WriteLine("Item Name\t Quantity\t Price");
            Console.WriteLine("=========\t ========\t =====");

            //Print items that were bought in transaction
            foreach (var record in transaction)
            {
                Console.WriteLine(String.Format("{0, -16} {1, -16} {2, -16}",
                record.ItemName, record.Quantity, record.ItemPrice));
            }
            Console.WriteLine("==============================");
            Console.WriteLine("Press 'ENTER' to continue...");
            Console.ReadLine();
            //Employee is returned
        }

        /// <summary>
        /// Prints a list of transactions made by customers
        /// Returns customer IDs
        /// </summary>
        /// <returns></returns>
        public static List<int> CustomerList(IEnumerable<User> users)
        {
            customerIDList.Clear();

            //Format our customer list
            Console.WriteLine("Here is the customer list:");
            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    "Entry", "First Name", "Last Name", "Phone Number"));
            Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    "=====", "==========", "=========", "============"));

            int entry = 1;
            foreach (var record in users)
            {
                Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    entry, record.First, record.Last, record.Phone));
                customerIDList.Add(record.Id);
                entry++;
            }
            Console.WriteLine("==============================");

            return customerIDList;
        }

        /// <summary>
        /// Pulls order history for specified customer
        /// Returns list of transaction IDs
        /// </summary>
        /// <param name="myEntry"></param>
        /// <returns></returns>
        public static List<string> CustomerTransactionHistory(IEnumerable<Transaction> transactions)
        {
            transList.Clear();

            //Format our transaction list
            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                    "Entry", "Transaction ID", "Store ID", "Total Price"));
            Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                    "=====", "==============", "========", "==========="));

            //Get transaction list from database and print
            
            int entry = 1;
            foreach (var record in transactions)
            {
                transList.Add(record.TransID);
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                    entry, record.TransID, record.StoreID, record.Price));
                entry++;
            }

            return transList;
        }

        /// <summary>
        /// Gets the name of the store for customer cart
        /// </summary>
        public static string GetStoreName(int storeEntry)
        {
            using SqlConnection connection = new(connectionString);

            string storeName = "";
            //Pull information from the entered store (Name)
            connection.Open();
            string getSelectedStore = $"SELECT * FROM StoreInfo WHERE StoreID = @storeID;";
            using SqlCommand readSelectedStore = new(getSelectedStore, connection);
            readSelectedStore.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            using SqlDataReader readStore = readSelectedStore.ExecuteReader();
            while (readStore.Read())
            {
                storeName = readStore.GetString(1);
            }
            connection.Close();

            return storeName;
        }

        /// <summary>
        /// Get a store's inventory and store inventory in a series of lists
        /// Return the lists
        /// </summary>
        public static List<int> GetStoreInventoryItemID(int storeEntry)
        {
            itemIDList.Clear();

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
                //Inventory is stored in list
                itemIDList.Add(readInventory.GetInt32(0));
            }
            connection.Close();

            return itemIDList;
        } //Item ID List
        public static List<string> GetStoreInventoryItemName(int storeEntry)
        {
            itemNameList.Clear();

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
                //Inventory is stored in list
                itemNameList.Add(readInventory.GetString(1));
            }
            connection.Close();

            return itemNameList;
        } //Item Name List
        public static List<int> GetStoreInventoryInStock(int storeEntry)
        {
            inStockList.Clear();

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
                //Inventory is stored in list
                inStockList.Add(readInventory.GetInt32(2));
            }
            connection.Close();

            return inStockList;
        } //Item In Stock List
        public static List<decimal> GetStoreInventoryPrice(int storeEntry)
        {
            priceList.Clear();

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
                //Inventory is stored in list
                decimal itemPrice = readInventory.GetDecimal(3);
                priceList.Add(itemPrice);
            }
            connection.Close();

            return priceList;
        } //Item Price List

        /// <summary>
        /// Our databases get updated.
        /// Store inventory is altered based on what the customer took
        /// The details of the transaction (customer ID, transaction ID, date and time) are added to the databased
        /// The items in the customer's cart are stored and logged in a database as part of the customer's trasaction
        /// </summary>
        public static void FinalizeTransaction(List<int> itemIDListNew, List<int> inStockListNew, int storeEntry, string transID, int userID, List<int> customerItemIDNew, List<int> customerQuantityNew, List<decimal> customerPriceNew)
        {
            using SqlConnection connection = new(connectionString);

            for (int i = 0; i < itemIDListNew.Count; i++) //Loop through remaining store inventory
            {
                //Updates store quantites based on what customer has added to their cart
                connection.Open();
                string updateStoreInv = "UPDATE StoreInventory SET InStock = @stock WHERE StoreID = @storeID AND ItemID = @itemID;";
                using SqlCommand newStoreInv = new(updateStoreInv, connection);
                newStoreInv.Parameters.Add("@stock", System.Data.SqlDbType.Int).Value = inStockListNew[i];
                newStoreInv.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
                newStoreInv.Parameters.Add("@itemID", System.Data.SqlDbType.Int).Value = itemIDListNew[i];
                newStoreInv.ExecuteNonQuery();
                connection.Close();
            }

            //Add details of transaction to database
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

            for (int i = 0; i < customerItemIDNew.Count; i++) //Loop through customer cart
            {
                //Add customer cart items to database
                connection.Open();
                string addTransDetails = "INSERT CustomerTransactionDetails (TransactionID, ItemID, Quantity, Price) " +
                    "VALUES (@transID, @itemID, @quantity, @price);";
                using SqlCommand newTransDetails = new(addTransDetails, connection);
                newTransDetails.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = transID;
                newTransDetails.Parameters.Add("@itemID", System.Data.SqlDbType.Int).Value = customerItemIDNew[i];
                newTransDetails.Parameters.Add("@quantity", System.Data.SqlDbType.Int).Value = customerQuantityNew[i];
                newTransDetails.Parameters.Add("@price", System.Data.SqlDbType.Money).Value = customerPriceNew[i];
                newTransDetails.ExecuteNonQuery();
                connection.Close();
            }

            //Clear all lists from next run through
            itemIDListNew.Clear();
            inStockListNew.Clear();
            customerItemIDNew.Clear();
            customerQuantityNew.Clear();
            customerPriceNew.Clear();
        }
    
        /// <summary>
        /// Search a customer by their first name
        /// </summary>
        /// <returns></returns>
        public static void PrintCustomerInfo(IEnumerable<User> users)
        {
            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "=======", "=========", "=========", "============", "============"));

            //Pull customer information matching first name entered
            foreach (var record in users)
            {
                Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    record.Id, record.First, record.Last, record.Phone, record.Employee));
            }
            Console.WriteLine("==============================");
        }
    }
}
