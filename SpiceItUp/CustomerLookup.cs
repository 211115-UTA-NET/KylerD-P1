using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Employees can pull a customers information by looking up their name
    /// All information is pulled from the database
    /// </summary>
    public static class CustomerLookup
    {
        private static bool exit = false;

        private static string? firstName;
        private static string? lastName;

        /// <summary>
        /// Employee chooses how they would like to lookup the customer (first or last name)
        /// </summary>
        public static void CustomerSearchOptions()
        {
            while (true)
            {
                exit = false;
                Console.WriteLine("Would you like to search by:");
                Console.WriteLine("1: First Name");
                Console.WriteLine("2: Last Name");
                Console.WriteLine("3: Or return to menu?");

                int userEntry;

                while (true) //Test to ensure user entry is valid
                {
                    string? mySelection = Console.ReadLine();
                    _ = int.TryParse(mySelection, out userEntry);
                    if (userEntry >= 1 && userEntry <= 3)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                switch (userEntry)
                {
                    case 1:
                        _ = SearchByFirstName(); //If employee chooses first name
                        break;
                    case 2:
                        _ = SearchByLastName(); //If employee chooses last name
                        break;
                    case 3: //Exit and return to account menu
                        exit = true;
                        break;
                }
            }
        }

        /// <summary>
        /// The employye can search the database by entering a first name
        /// </summary>
        public static async Task SearchByFirstName()
        {
            while (true)
            {
                Console.WriteLine("Enter a first name:");
                firstName = Console.ReadLine();
                if (firstName == null) //If entry is null
                {
                    Console.WriteLine("Invalid entry. Please try again");
                }
                else
                {
                    try //Try to pull customer information
                    {
                        SpiceItUpService service = new SpiceItUpService(Program.server);
                        List<User> users = await service.GetUserFirstName(firstName);
                        PrintResults.PrintCustomerInfo(users);
                        break;
                    }
                    catch (Exception) //If we run into an error while accessing database
                    {
                        Console.WriteLine("There was an error retrieving the customer information.");
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// The employye can search the database by entering a last name
        /// </summary>
        public static async Task SearchByLastName()
        {
            while (true)
            {
                Console.WriteLine("Enter a last name:");
                lastName = Console.ReadLine();
                if (lastName == null) //If entry is null
                {
                    Console.WriteLine("Invalid entry. Please try again");
                }
                else
                {
                    try //Try to pull customer information
                    {
                        SpiceItUpService service = new SpiceItUpService(Program.server);
                        List<User> users = await service.GetUserLastName(lastName);
                        PrintResults.PrintCustomerInfo(users);
                        break;
                    }
                    catch (Exception) //If we run into an error while accessing database
                    {
                        Console.WriteLine("There was an error retrieving the customer information.");
                    }
                    break;
                }
            }
        }
    }
}
