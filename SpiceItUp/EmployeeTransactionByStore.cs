using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Employees can lookup a customers transaction based on what store it was ordered from
    /// </summary>
    public class EmployeeTransactionByStore : IViewTransaction
    {
        /// <summary>
        /// A stores' transactions are stored in a list
        /// </summary>
        public static List<string> transList = new List<string>();

        private static bool exit = false;
        private static int storeEntry;
        private static int userEntry;

        /// <summary>
        /// Employee is given a list of stores.
        /// Employe is then given a list of transactions made at the selected store
        /// </summary>
        public static async void StoreSelection()
        {
            exit = false;
            while (true)
            {
                Console.WriteLine("Enter a store number to view the order history:");

                try
                {
                    SpiceItUpService service = new SpiceItUpService(SpiceItUp.Program.server);
                    List<Store> stores = await service.GetStoreList();
                    SpiceItUp.PrintResults.PrintStoreList(stores);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find stores. Returning to main menu.");
                    break;
                }

                while (true) //Test to ensure user entry is valid
                {
                    string? storeSelection = Console.ReadLine();
                    bool validEntry = int.TryParse(storeSelection, out storeEntry);
                    if (validEntry == true && storeEntry > 100 && storeEntry < 105) //If selected store is valid
                    {
                        TransactionHistory(storeEntry); //Print off transactions in the database from entered store
                        break;
                    }
                    else //If there is an unknown error
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                Console.WriteLine("Would you like to check another store's order history? (Y/N)"); //Employee can shoose another store to view order history from
                string? checkNewStore = Console.ReadLine();
                if ("Y" != checkNewStore?.ToUpper()) //Return to account main menu
                    break;
            }
        }

        /// <summary>
        /// The basic transaction information is printed for the selected store.
        /// The employee has the option to view a transaction more in depth
        /// </summary>
        public static async void TransactionHistory(int myEntry)
        {
            exit = false;
            while (exit == false)
            {
                transList.Clear();

                try
                {
                    SpiceItUpService service = new SpiceItUpService(SpiceItUp.Program.server);
                    List<Transaction> transactions = await service.GetStoreTransactionList(myEntry);
                    transList = SpiceItUp.PrintResults.PrintStoreTransactionList(transactions, myEntry);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error pulling store transactions. Returning to store list.");
                    break;
                }

                if (transList.Count == 0) //If the store has no transactions, return to store list
                {
                    Console.WriteLine("Going back. This store does not have a transaction history.");
                    break;
                }
                Console.WriteLine($"To view an order's specific details, enter the Entry number.");
                Console.WriteLine("Otherwise, enter 0 to go back.");

                while (true) //The employee can now view a store's transaction in more detail, if they wish
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (transList.Count >= userEntry && userEntry > 0) //If a transaction selectionn is valid
                    {
                        SpiceItUpService service2 = new SpiceItUpService(SpiceItUp.Program.server);
                        userEntry = userEntry - 1;
                        string transactionNum = transList[userEntry];
                        List<Transaction> transaction = await service2.DetailedTransaction(transactionNum);
                        SpiceItUp.PrintResults.DetailedTransaction(transaction); //View the transaction in more detail
                        break;
                    }
                    else if (userEntry == 0) //If employee enters 0, return to store list
                    {
                        exit = true;
                        break;
                    }
                    else //If there is an unknown error
                        Console.WriteLine("Invalid selection. Please try again.");
                }
            }
        }
    }
}
