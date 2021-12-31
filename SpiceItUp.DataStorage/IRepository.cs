using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    internal interface IRepository
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

        private static string? storeName;
        private static int userID;
        private static string? firstName;
        private static string? lastName;
        private static double phoneNumber;
        private static string? isEmployee;

        /// <summary>
        /// The information entered by user is written to our database
        /// This information can be pulled and used to login to an account when entered in correctly
        /// </summary>
        public static void AddNewCustomer(string newUsername, string newPassword, string firstName, string lastName, string phoneNumber) { }

        /// <summary>
        /// Prints the basic summary of all store locations
        /// </summary>
        public static void PrintStoreList() { }

        /// <summary>
        /// We will attempt to pull the inventory from entered store
        /// Store inventory is pulled from database and formatted accordingly
        /// </summary>
        public static void PullStoreInfo(int storeEntry) { }

        /// <summary>
        /// Pulls the order history from the specified store
        /// A list of transaction IDs is returned
        /// </summary>
        /// <param name="myEntry"></param>
        /// <returns></returns>
        public static List<string> StoreTransactionList(int myEntry) { return transList; }

        /// <summary>
        /// A transaction is printed off in more detail based on employee selection
        /// </summary>
        public static void DetailedTransaction(int userEntry) { }

        /// <summary>
        /// Prints a list of transactions made by customers
        /// Returns customer IDs
        /// </summary>
        /// <returns></returns>
        public static List<int> CustomerTransactionList() { return customerIDList; }

        /// <summary>
        /// Pulls order history for specified customer
        /// Returns list of transaction IDs
        /// </summary>
        /// <param name="myEntry"></param>
        /// <returns></returns>
        public static List<string> CustomerTransactionHistory(int customerID) { return transList; }

        /// <summary>
        /// Gets the name of the store for customer cart
        /// </summary>
        public static string GetStoreName(int storeEntry) { return storeName; }

        /// <summary>
        /// Get a store's inventory and store inventory in a series of lists
        /// Return the lists
        /// </summary>
        public static List<int> GetStoreInventoryItemID(int storeEntry){ return itemIDList; }
        public static List<string> GetStoreInventoryItemName(int storeEntry) { return itemNameList; }
        public static List<int> GetStoreInventoryInStock(int storeEntry){return inStockList; }
        public static List<decimal> GetStoreInventoryPrice(int storeEntry){return priceList; }

        /// <summary>
        /// Our databases get updated.
        /// Store inventory is altered based on what the customer took
        /// The details of the transaction (customer ID, transaction ID, date and time) are added to the databased
        /// The items in the customer's cart are stored and logged in a database as part of the customer's trasaction
        /// </summary>
        public static void FinalizeTransaction(List<int> itemIDListNew, List<int> inStockListNew, int storeEntry, string transID, int userID, List<int> customerItemIDNew, List<int> customerQuantityNew, List<decimal> customerPriceNew) { }

        /// <summary>
        /// Search a customer by their first name
        /// </summary>
        /// <returns></returns>
        public static void SearchCustomerFirstName(string firstName) { }

        /// <summary>
        /// Search a custome rby their last name
        /// </summary>
        /// <returns></returns>
        public static void SearchCustomerLastName(string lastName) { }

        /// <summary>
        /// Based on information in our database, are the entries linked to an accounnt?
        /// If so, customer is logged in.
        /// If anything fails, we return to the main program class
        /// </summary>
        public static int GetLoginUserID(string username, string password) { return userID; }

        /// <summary>
        /// Get user info based off userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetLoginFirstName(int userID) { return firstName; }
        public static string GetLoginLastName(int userID) { return lastName; }
        public static double GetLoginPhoneNumber(int userID) { return phoneNumber; }
        public static string GetLoginIsEmployee(int userID) { return isEmployee; }
    }
}