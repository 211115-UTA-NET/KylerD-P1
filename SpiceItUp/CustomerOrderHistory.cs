using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Customers can view their order history
    /// </summary>
    public class CustomerOrderHistory
    {
        private static bool exit = false;
        public static List<string> transList = new List<string>();
        private static int userEntry;

        /// <summary>
        /// The basic details of a customers transaction are automatically printed off.
        /// Customers can select a transaction to view it more in detail
        /// </summary>
        /// <param name="myUserID"></param>
        public static async void CustomerTransactionHistory(int myUserID)
        {
            exit = false;
            while (exit == false)
            {
                transList.Clear();


                Console.WriteLine("Here is your order History:");
                try
                {
                    SpiceItUpService service = new SpiceItUpService(SpiceItUp.Program.server);
                    List<Transaction> transactions = await service.GetCustomerTransactionList(myUserID);
                    transList = SpiceItUp.PrintResults.CustomerTransactionHistory(transactions);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to print off your transaction history. Returning to menu.");
                    break;
                }

                if (transList.Count == 0) //If there are no previous transactions
                {
                    Console.WriteLine("Returning to your menu. You do not have a transaction history.");
                    break;
                }

                Console.WriteLine($"To view an order's specific details, enter the Entry number.");
                Console.WriteLine("Otherwise, enter 0 to exit.");

                //User can select a transaction from the list to view more details
                while (true)
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (validEntry == true && transList.Count >= userEntry && userEntry > 0) //If entery is valid
                    {
                        SpiceItUpService service2 = new SpiceItUpService(SpiceItUp.Program.server);
                        userEntry--;
                        string transactionNum = transList[userEntry];
                        List<Transaction> transaction = await service2.DetailedTransaction(transactionNum);
                        SpiceItUp.PrintResults.DetailedTransaction(transaction); //View the transaction in more detail
                        break;
                    }
                    else if (userEntry == 0) //If customer wishes to exit
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
