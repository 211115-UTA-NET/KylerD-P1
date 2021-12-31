using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpLogic
{
    /// <summary>
    /// User will enter their login information
    /// If valid, the user will be able to login to their account
    /// </summary>
    public class AccountLogin
    {
        private static int userID = 0;
        public static string firstName = "";
        public static string lastName = "";
        private static double phoneNumber = 0;
        public static string isEmployee = "";

        private static string? enteredUsername;
        private static string? enteredPassword;

        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        /// <summary>
        /// Customer is prompted to enter their username and password
        /// </summary>
        public static void LoginManager()
        {
            Console.WriteLine("Lets get you logged in!");
            
            // User enters their username
            while (true)
            {
                Console.WriteLine("Username:");
                enteredUsername = Console.ReadLine();
                if (enteredUsername == null)
                    Console.WriteLine("Invalid entry.");
                else
                    break;
            }

            // User enters their password
            while (true)
            {
                Console.WriteLine("Password:");
                enteredPassword = Console.ReadLine();
                if (enteredPassword == null)
                    Console.WriteLine("Invalid entry.");
                else
                    break;
            }

            try
            {
                TestEntries(enteredUsername, enteredPassword); //Can we log into an account?
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error logging you in. Please try again.");
            }

            if (isEmployee == "FALSE") // Is this a customer account?
            {
                CustomerAccount customerLogin;
                customerLogin = new CustomerAccount(userID, firstName, lastName, phoneNumber);
                customerLogin.UserOptions();
            }
            else if (isEmployee == "TRUE") // Is this an employee account?
            {
                EmployeeAccount employeeLogin;
                employeeLogin = new EmployeeAccount(userID, firstName, lastName, phoneNumber);
                employeeLogin.UserOptions();
            }
            else
                Console.WriteLine("Invalid account. Please try again");
        }

        /// <summary>
        /// Based on information in our database, are the entries linked to an accounnt?
        /// If so, customer is logged in.
        /// If anything fails, we return to the main program class
        /// </summary>
        public static void TestEntries(string username, string password)
        {
            //If username and password is a valid entry, pull a UserID
            userID = SpiceItUpDataStorage.SqlRepository.GetLoginUserID(username, password);

            //Get user information based on valid UserID
            firstName = SpiceItUpDataStorage.SqlRepository.GetLoginFirstName(userID);
            lastName = SpiceItUpDataStorage.SqlRepository.GetLoginLastName(userID);
            phoneNumber = SpiceItUpDataStorage.SqlRepository.GetLoginPhoneNumber(userID);
            isEmployee = SpiceItUpDataStorage.SqlRepository.GetLoginIsEmployee(userID);
        }
    }
}
