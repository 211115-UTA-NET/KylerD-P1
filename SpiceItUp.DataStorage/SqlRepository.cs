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

        /// <summary>
        /// Information from the database reguarding store inventory is stored in lists
        /// </summary>
        private static List<int> itemIDList = new List<int>();
        private static List<string> itemNameList = new List<string>();
        private static List<int> inStockList = new List<int>();
        private static List<decimal> priceList = new List<decimal>();

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
    }
}
