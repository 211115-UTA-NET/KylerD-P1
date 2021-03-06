using SpiceItUp.Dtos;
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
    public static class LocationInventory
    {
        private static int storeEntry;

        /// <summary>
        /// Print off a list of stores by pulling store information from the database.
        /// The user can then sleect a store to view it's inventory
        /// </summary>
        public static async Task StoreSelection()
        {
            while (true)
            {
                Console.WriteLine("Enter a store number to view their inventory:");

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
                    storeEntry = ValidStore(storeSelection);
                    if (storeEntry > 100)
                        break;
                }

                try
                {
                    SpiceItUpService service2 = new SpiceItUpService(SpiceItUp.Program.server);
                    List<Store> inventory = await service2.GetStoreInventory(storeEntry);
                    SpiceItUp.PrintResults.PrintStoreInfo(inventory, storeEntry); //Can we pull the store's inventory information based on user input?
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

        public static int ValidStore(string? selection)
        {
            _ = int.TryParse(selection, out int storeEntry);
            if (storeEntry > 100 && storeEntry < 105)
            {
                return storeEntry;
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
                return 0;
            }
        }
    }
}