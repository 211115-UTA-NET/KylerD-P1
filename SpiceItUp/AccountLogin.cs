using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// User will enter their login information
    /// If valid, the user will be able to login to their account
    /// </summary>
    public class AccountLogin
    {
        private static string? enteredUsername;
        private static string? enteredPassword;

        /// <summary>
        /// Customer is prompted to enter their username and password
        /// </summary>
        public static async Task LoginManager()
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

            IEnumerable<User> info = new List<User>();

            try
            {
                SpiceItUpService service = new SpiceItUpService(SpiceItUp.Program.server);
                IEnumerable<User> login = await service.GetLoginInfo(enteredUsername, enteredPassword);
                int loginID = 0;
                foreach (var record in login)
                {
                    loginID = record.Id;
                }
                info = await service.GetUserInfo(loginID);
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error logging you in. Please try again.");
            }

            foreach (var record in info)
            {
                if (record.Employee == "FALSE") // Is this a customer account?
                {
                    CustomerAccount customerLogin;
                    customerLogin = new CustomerAccount(record.Id, record.First, record.Last, record.Phone);
                    customerLogin.UserOptions();
                    break;
                }
                else if (record.Employee == "TRUE") // Is this an employee account?
                {
                    EmployeeAccount employeeLogin;
                    employeeLogin = new EmployeeAccount(record.Id, record.First, record.Last, record.Phone);
                    employeeLogin.UserOptions();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid account. Please try again");
                    break;
                }
            }
        }
    }
}
