using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Employees can lookup a customers transaction based on a list of customers
    /// </summary>
    public class EmployeeTransactionByCustomer : IViewTransaction
    {
        private static bool exit = false;
        private static bool goBack = false;
        private static int userEntry;
        private static int userEntry2;

        /// <summary>
        /// Customer's basic info is stored in lists
        /// </summary>
        private static List<int> customerIDList = new List<int>();
        private static List<string> transList = new List<string>();

        /// <summary>
        /// The database prints off a list of customers for the employee to choose from
        /// </summary>
        public async static Task SelectACustomer()
        {
            exit = false;
            while (exit == false)
            {
                customerIDList.Clear();

                try
                {
                    SpiceItUpService service = new SpiceItUpService(SpiceItUp.Program.server);
                    List<User> users = await service.GetCustomerList();
                    customerIDList = PrintResults.CustomerList(users);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to print off customer transaction list. Returning to main menu.");
                    break;
                }

                Console.WriteLine($"To view a customer's order history, enter the Entry number.");
                Console.WriteLine("Otherwise, enter 0 to exit.");

                while (true)
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (validEntry == true && customerIDList.Count >= userEntry && userEntry > 0) //If employee chooses a valid customer
                    {
                        userEntry--;
                        _ = TransactionHistory(userEntry); //Print customer's transaction history
                        break;
                    }
                    else if (userEntry == 0) //If customer wishes to return to account main menu
                    {
                        exit = true;
                        Console.WriteLine("return");
                        break;
                    }
                    else //If we get an unknown error
                        Console.WriteLine("Invalid selection. Please try again.");
                }
            }
        }

        /// <summary>
        /// Once the employee chooses a customer, that customer's transaction list is printed off.
        /// The employee can choose to view a transaction in more details if they wish
        /// </summary>
        public static async Task TransactionHistory(int myEntry)
        {
            goBack = false;

            while (goBack == false)
            {
                transList.Clear();

                try
                {
                    int customerID = customerIDList[myEntry];
                    SpiceItUpService service = new SpiceItUpService(SpiceItUp.Program.server);
                    List<Transaction> transactions = await service.GetCustomerTransactionList(customerID);
                    transList = PrintResults.CustomerTransactionHistory(transactions);
                }
                catch(Exception)
                {
                    Console.WriteLine("Failed to print off the customer's transaction history. Returning to customer selection)");
                    break;
                }

                if (transList.Count == 0) //If the customer does not have any transactions to view, return to customer selection
                {
                    Console.WriteLine("Going back. This customer does not have a transaction history.");
                    break;
                }
                Console.WriteLine($"To view an order's specific details, enter the Entry number.");
                Console.WriteLine("Otherwise, enter 0 to go back.");

                while (true) //The employee is prompted to choose a transaction to view in more detail
                {
                    string? mySelection = Console.ReadLine();
                    bool validEntry = int.TryParse(mySelection, out userEntry2);
                    if (validEntry == true && transList.Count >= userEntry2 && userEntry2 > 0) //If employee chooses a transaction
                    {
                        SpiceItUpService service2 = new SpiceItUpService(Program.server);
                        userEntry2--;
                        string transactionNum = transList[userEntry2];
                        List<Transaction> transaction = await service2.DetailedTransaction(transactionNum);
                        PrintResults.DetailedTransaction(transaction); //View the transaction in more detail
                        break;
                    }
                    else if (userEntry2 == 0) //Return to customer list
                    {
                        goBack = true;
                        break;
                    }
                    else //If there is an unknown error
                        Console.WriteLine("Invalid selection. Please try again.");
                }
            }
        }
    }
}
