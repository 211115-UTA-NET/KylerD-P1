using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public static class PrintResults
    {
        private static List<string> transList = new List<string>();

        private static List<int> customerIDList = new List<int>();

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
            bool breakOnce = true;

            foreach (var record in transaction)
            {
                
                Console.WriteLine("==============================");
                Console.WriteLine($"Transaction ID: {record.TransID}");
                Console.WriteLine($"Store {record.StoreID}: {record.StoreName}");
                Console.WriteLine($"Name: {record.FirstName} {record.LastName}");
                Console.WriteLine($"Time: {record.Timestamp}");
                Console.WriteLine($"Total: {record.TotalPrice}");
                Console.WriteLine("==============================");
                if(breakOnce == true)
                {
                    break;
                }
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
