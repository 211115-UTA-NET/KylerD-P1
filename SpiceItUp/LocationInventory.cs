using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// Customers and employees can view a store's inventory.
    /// Customers can view the store's inventory before they begin an order.
    /// </summary>
    public class LocationInventory
    {
        private static int storeEntry;

        /// <summary>
        /// Print off a list of stores by pulling store information from the database.
        /// The user can then sleect a store to view it's inventory
        /// </summary>
        public static void StoreSelection()
        {
            while (true)
            {
                Console.WriteLine("Enter a store number to view their inventory:");

                try
                {
                    SpiceItUp.SqlRepository.PrintStoreList();
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
                    if (validEntry == true && storeEntry > 100 && storeEntry < 105)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                try
                {
                    SpiceItUp.SqlRepository.PullStoreInfo(storeEntry); //Can we pull the store's inventory information based on user input?
                }
                catch (Exception) //If we fail to pull store inventory
                {
                    Console.WriteLine("Unable to pull store information");
                }

                Console.WriteLine("Would you like to check another store's inventory? (Y/N)"); //If customer wishes to check another store's inventory, reprint store lists
                string? checkNewStore = Console.ReadLine();
                if ("Y" != checkNewStore?.ToUpper()) //If customer does not wish to check another store's inventory, break the loop and return to account main menu
                    break;
            }
        }
    }
}