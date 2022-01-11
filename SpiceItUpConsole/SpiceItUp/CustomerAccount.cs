using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// A user directed to this class is flagged as a customer.
    /// Customers are given options for how to proceed.
    /// </summary>
    public class CustomerAccount
    {
        protected int userID;
        protected string? firstName;
        protected string? lastName;
        protected double phoneNumber;

        private static bool logout = false;

        /// <summary>
        /// Constructor to get the basic information from customer.
        /// This information was pulled from the database when the customer logged in.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        public CustomerAccount(int userID, string? firstName, string? lastName, double phoneNumber)
        {
            this.userID = userID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
        }

        /// <summary>
        /// Customers are given different options for how to proceed.
        /// </summary>
        public void CustomerOptions()
        {
            while (logout == false)
            {
                Console.WriteLine($"Welcome, {firstName} {lastName[0]}! What would you like to do?");
                Console.WriteLine("1: Start a new order");
                Console.WriteLine("2: View order history");
                Console.WriteLine("3: View store inventory");
                Console.WriteLine("4: Log out");

                int userEntry;

                while (true) //Test to ensure user entry is valid
                {
                    string? mySelection = Console.ReadLine();
                    userEntry = ValidEntry(mySelection);
                    if (userEntry > 0)
                        break;
                }

                SelectedOption(userEntry, userID);
            }
        }

        public static int ValidEntry(string? selection)
        {
            _ = int.TryParse(selection, out int userEntry);
            if (userEntry >= 1 && userEntry <= 4)
            {
                return userEntry; //Break when valid
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
                return 0;
            }
        }

        public static void SelectedOption(int userEntry, int userID)
        {
            switch (userEntry)
            {
                case 1:
                    _ = SpiceItUp.CustomerOrder.StoreSelection(userID); //Start a new order
                    break;
                case 2:
                    _ = SpiceItUp.CustomerOrderHistory.CustomerTransactionHistory(userID); //View order history
                    break;
                case 3:
                    _ = SpiceItUp.LocationInventory.StoreSelection(); //View store inventory
                    break;
                case 4: //Log out of account
                    Console.WriteLine("Goodbye!");
                    logout = true;
                    break;
            }
        }
    }
}
