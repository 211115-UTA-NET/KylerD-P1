using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class SqlRepository : IRepository
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
        //private static List<int> customerItemID = new List<int>();
        //private static List<string> customerItemName = new List<string>();
        //private static List<int> customerQuantity = new List<int>();
        //private static List<decimal> customerPrice = new List<decimal>();

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
        public static void PrintStoreList()
        {
            using SqlConnection connection = new(connectionString);

            //Get list of stores from database
            connection.Open();
            string getStoreInfo = "SELECT * FROM StoreInfo ORDER BY StoreID;";
            using SqlCommand readStoreInfo = new(getStoreInfo, connection);
            using SqlDataReader reader = readStoreInfo.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Store {reader.GetInt32(0)}: {reader.GetString(1)}");
            }
            connection.Close();
        }

        /// <summary>
        /// We will attempt to pull the inventory from entered store
        /// Store inventory is pulled from database and formatted accordingly
        /// </summary>
        public static void PullStoreInfo(int storeEntry)
        {
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
                decimal itemPrice = readInventory.GetDecimal(2);
                string price = String.Format("{0:0.00}", itemPrice);
                Console.WriteLine(String.Format("{0, -16} {1, -15} {2, -16}", readInventory.GetString(0), readInventory.GetInt32(1), $"${price}"));
            }
            connection.Close();
        }

        /// <summary>
        /// Pulls the order history from the specified store
        /// A list of transaction IDs is returned
        /// </summary>
        /// <param name="myEntry"></param>
        /// <returns></returns>
        public static List<string> StoreTransactionList(int myEntry)
        {
            transList.Clear();

            using SqlConnection connection = new(connectionString);

            //Format our transactions
            Console.WriteLine($"Order history for store {myEntry}");
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
            storeOrderHistory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = myEntry;
            using SqlDataReader reader = storeOrderHistory.ExecuteReader();
            int entry = 1;
            while (reader.Read())
            {
                transList.Add(reader.GetString(0));
                string price = String.Format("{0:0.00}", reader.GetDecimal(3));
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -10} {4, -10}",
                    entry, reader.GetString(0), reader.GetString(1), reader.GetString(2), $"${price}"));
                entry++;
            }
            connection.Close();

            return transList;
        }

        /// <summary>
        /// A transaction is printed off in more detail based on employee selection
        /// </summary>
        public static void DetailedTransaction(int userEntry)
        {
            int entryList = userEntry - 1;
            string detailedTransID = transList[entryList];

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
            readOpening.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = detailedTransID;
            using SqlDataReader myReader = readOpening.ExecuteReader();
            while (myReader.Read())
            {
                string price = String.Format("{0:0.00}", myReader.GetDecimal(6));
                Console.WriteLine("==============================");
                Console.WriteLine($"Transaction ID: {myReader.GetString(0)}");
                Console.WriteLine($"Store {myReader.GetInt32(1)}: {myReader.GetString(2)}");
                Console.WriteLine($"Name: {myReader.GetString(4)} {myReader.GetString(5)}");
                Console.WriteLine($"Time: {myReader.GetString(3)}");
                Console.WriteLine($"Total: ${price}");
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
            readDetails.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = detailedTransID;
            using SqlDataReader detailReader = readDetails.ExecuteReader();
            while (detailReader.Read())
            {
                string price = String.Format("{0:0.00}", detailReader.GetDecimal(2));
                Console.WriteLine(String.Format("{0, -16} {1, -16} {2, -16}",
                detailReader.GetString(0), detailReader.GetInt32(1), $"${price}"));
            }
            Console.WriteLine("==============================");
            connection.Close();
            Console.WriteLine("Press 'ENTER' to continue...");
            Console.ReadLine();
            //Employee is returned
        }

        /// <summary>
        /// Prints a list of transactions made by customers
        /// Returns customer IDs
        /// </summary>
        /// <returns></returns>
        public static List<int> CustomerTransactionList()
        {
            customerIDList.Clear();

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
                customerIDList.Add(readCustomers.GetInt32(0));
                Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    entry, readCustomers.GetString(1), readCustomers.GetString(2), readCustomers.GetInt64(3)));
                entry++;
            }
            connection.Close();

            return customerIDList;
        }

        /// <summary>
        /// Pulls order history for specified customer
        /// Returns list of transaction IDs
        /// </summary>
        /// <param name="myEntry"></param>
        /// <returns></returns>
        public static List<string> CustomerTransactionHistory(int customerID)
        {
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
                transList.Add(reader.GetString(0));
                string price = String.Format("{0:0.00}", reader.GetDecimal(2));
                Console.WriteLine(String.Format("{0, -7} {1, -17} {2, -10} {3, -7}",
                    entry, reader.GetString(0), reader.GetInt32(1), $"${price}"));
                entry++;
            }
            connection.Close();

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
        public static void SearchCustomerFirstName(string firstName)
        {
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
                Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3), reader.GetString(4)));
            }
            Console.WriteLine("==============================");
        }

        /// <summary>
        /// Search a custome rby their last name
        /// </summary>
        /// <returns></returns>
        public static void SearchCustomerLastName(string lastName)
        {
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
            {Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3), reader.GetString(4)));
            }
            Console.WriteLine("==============================");
        }

        /// <summary>
        /// Based on information in our database, are the entries linked to an accounnt?
        /// If so, customer is logged in.
        /// If anything fails, we return to the main program class
        /// </summary>
        public static int GetLoginUserID(string username, string password)
        {
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
            }
            connection.Close();

            return userID;
        }

        /// <summary>
        /// Get user info based off userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetLoginFirstName(int userID)
        {
            string firstName = "";

            using SqlConnection connection = new(connectionString);

            connection.Open();
            string getUserInfo = $"SELECT * FROM UserInformation WHERE UserID = @validUserID;";
            using SqlCommand readUserInfo = new(getUserInfo, connection);
            readUserInfo.Parameters.Add("@validUserID", System.Data.SqlDbType.Int).Value = userID;
            using SqlDataReader userReader = readUserInfo.ExecuteReader();
            while (userReader.Read())
            {
                firstName = userReader.GetString(1);
            }
            connection.Close();

            return firstName;
        }
        public static string GetLoginLastName(int userID)
        {
            string lastName = "";

            using SqlConnection connection = new(connectionString);

            connection.Open();
            string getUserInfo = $"SELECT * FROM UserInformation WHERE UserID = @validUserID;";
            using SqlCommand readUserInfo = new(getUserInfo, connection);
            readUserInfo.Parameters.Add("@validUserID", System.Data.SqlDbType.Int).Value = userID;
            using SqlDataReader userReader = readUserInfo.ExecuteReader();
            while (userReader.Read())
            {
                lastName = userReader.GetString(2);
            }
            connection.Close();

            return lastName;
        }
        public static double GetLoginPhoneNumber(int userID)
        {
            double phoneNumber = 0;

            using SqlConnection connection = new(connectionString);

            connection.Open();
            string getUserInfo = $"SELECT * FROM UserInformation WHERE UserID = @validUserID;";
            using SqlCommand readUserInfo = new(getUserInfo, connection);
            readUserInfo.Parameters.Add("@validUserID", System.Data.SqlDbType.Int).Value = userID;
            using SqlDataReader userReader = readUserInfo.ExecuteReader();
            while (userReader.Read())
            {
                phoneNumber = userReader.GetInt64(3);
            }
            connection.Close();

            return phoneNumber;
        }
        public static string GetLoginIsEmployee(int userID)
        {
            string isEmployee = "";

            using SqlConnection connection = new(connectionString);

            connection.Open();
            string getUserInfo = $"SELECT * FROM UserInformation WHERE UserID = @validUserID;";
            using SqlCommand readUserInfo = new(getUserInfo, connection);
            readUserInfo.Parameters.Add("@validUserID", System.Data.SqlDbType.Int).Value = userID;
            using SqlDataReader userReader = readUserInfo.ExecuteReader();
            while (userReader.Read())
            {
                isEmployee = userReader.GetString(4);
            }
            connection.Close();

            return isEmployee;
        }
    }
}
