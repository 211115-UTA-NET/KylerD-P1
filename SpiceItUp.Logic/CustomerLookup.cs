﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpLogic
{
    /// <summary>
    /// Employees can pull a customers information by looking up their name
    /// All information is pulled from the database
    /// </summary>
    public class CustomerLookup
    {
        private static bool exit = false;

        public static string? firstName;
        public static string? lastName;

        /// <summary>
        /// Employee chooses how they would like to lookup the customer (first or last name)
        /// </summary>
        public static void CustomerSearchOptions()
        {
            while (exit == false)
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
                    bool validEntry = int.TryParse(mySelection, out userEntry);
                    if (validEntry == true && userEntry >= 1 && userEntry <= 3)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                switch (userEntry)
                {
                    case 1:
                        SearchByFirstName(); //If employee chooses first name
                        break;
                    case 2:
                        SearchByLastName(); //If employee chooses last name
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
        public static void SearchByFirstName()
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
                        SpiceItUpDataStorage.SqlRepository.SearchCustomerFirstName(firstName);
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
        public static void SearchByLastName()
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
                        SpiceItUpDataStorage.SqlRepository.SearchCustomerLastName(lastName);
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
