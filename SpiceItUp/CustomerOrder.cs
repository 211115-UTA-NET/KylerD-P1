using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    /// <summary>
    /// This class manages a customer order.
    /// Orders are restricted based on the store inventory.
    /// </summary>
    public class CustomerOrder
    {
        private static int storeEntry;
        private static string? storeName;
        private static bool exit = false;
        private static int userID;

        /// <summary>
        /// Information from the database reguarding store inventory is stored in lists
        /// </summary>
        private static List<int> itemIDList = new List<int>();
        private static List<string> itemNameList = new List<string>();
        private static List<int> inStockList = new List<int>();
        private static List<decimal> priceList = new List<decimal>();

        /// <summary>
        /// Information reguarding what the customer has added or removed from their cart is stored in lists
        /// </summary>
        private static List<int> customerItemID = new List<int>();
        private static List<string> customerItemName = new List<string>();
        private static List<int> customerQuantity = new List<int>();
        private static List<decimal> customerPrice = new List<decimal>();

        /// <summary>
        /// Customer selects the store in which they want to order from
        /// </summary>
        /// <param name="myUserID"></param>
        public static async Task StoreSelection(int myUserID)
        {
            userID = myUserID;
            exit = false;
            while (exit == false)
            {
                Console.WriteLine("Enter a store number to begin shopping:");

                try
                {
                    SpiceItUpService service = new SpiceItUpService(Program.server);
                    List<Store> stores = await service.GetStoreList();
                    PrintResults.PrintStoreList(stores);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to pull store info. Returning to menu.");
                    break;
                }

                while (true) //Test to ensure user entry is valid
                {
                    string? storeSelection = Console.ReadLine();
                    bool validEntry = int.TryParse(storeSelection, out storeEntry);
                    if (validEntry == true && storeEntry > 100)
                    {
                        break; //Break when valid
                    }
                    else
                        Console.WriteLine("Invalid selection. Please try again.");
                }

                try
                {
                    _ = PullStoreInfo(); //If store is valid, we will try to pull the store inventory and store it in a series of lists
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to pull store information.");
                }

                AddToCart(); //Customer is automatically directed to begin adding items to their cart
            }
        }

        /// <summary>
        /// Based on user entry, we pull all store information reguarding name and inventory of store
        /// </summary>
        public static async Task PullStoreInfo()
        {
            SpiceItUpService service = new(SpiceItUp.Program.server);

            itemIDList.Clear();
            itemNameList.Clear();
            inStockList.Clear();
            priceList.Clear();

            customerItemID.Clear();
            customerItemName.Clear();
            customerQuantity.Clear();
            customerPrice.Clear();

            //Pull information from the entered store (Name)
            List<Store> storeInfo = await service.GetStoreName(storeEntry);
            foreach(var record in storeInfo)
            {
                storeName = record.StoreName;
            }

            //Begin pulling the inventory from the selected store and storing the info in lists
            List<Store> storeCart = await service.GetCartStoreInventory(storeEntry);
            foreach(var record in storeCart)
            {
                itemIDList.Add(record.ItemID);
                itemNameList.Add(record.ItemName);
                inStockList.Add(record.ItemQuantity);
                priceList.Add(record.ItemPriceDecimal);
            }
        }

        /// <summary>
        /// Customers can add items to their cart based on what the store has in stock.
        /// Lists are updated accordingly. (Store inventory and customer cart)
        /// </summary>
        public static void AddToCart()
        {
            while (exit == false)
            {
                //Print off what the store has in stock
                Console.WriteLine($"Store {storeEntry}: {storeName}");
                Console.WriteLine("================================");
                Console.WriteLine("Item ID\t Item Name\t In Stock\t Price");
                Console.WriteLine("=======\t =========\t ========\t =====");
                for (int i = 0; i < itemIDList.Count; i++) //Print off information from lists in which store inventory has been stored in
                {
                    string price = String.Format("{0:0.00}", priceList[i]);
                    Console.WriteLine(String.Format("{0, -8} {1, -15} {2, -15} {3, -16}",
                        itemIDList[i], itemNameList[i], inStockList[i], $"${price}"));
                }

                Console.WriteLine("Enter an Item ID to add to your cart, or enter 0 to view your cart.");
                Console.WriteLine("Type 'EXIT' to cancel your order and return to your account menu.");
                while (exit == false)
                {
                    string? adding = Console.ReadLine(); //Customer enters which item ID they would like to add to their cart
                    bool validEntry = int.TryParse(adding, out int itemToAdd);
                    itemToAdd--;
                    if (validEntry == true && itemToAdd > -1 && itemToAdd < itemIDList.Count && inStockList[itemToAdd] != 0) //If our item ID entry is valid
                    {
                        Console.WriteLine("Enter a quantity to add to cart:");
                        while (exit == false)
                        {
                            string? quantityString = Console.ReadLine(); //Customers will now enter a quantity
                            bool validQuantity = int.TryParse(quantityString, out int quantity);
                            bool failedEntry = false;
                            bool sameEntry = false;
                            bool maxQuantity = false;
                            if (validQuantity == true && quantity > 0 && quantity < 11 && quantity <= inStockList[itemToAdd]) //If quantity entered is valid
                            {
                                for (int j = 0; j < customerItemID.Count; j++) //Loop through current customer cart to find duplicate Item IDs
                                {
                                    itemToAdd++;
                                    if (itemToAdd == customerItemID[j]) //If a customer has already added the item to their cart, the quantites are combined
                                    {
                                        itemToAdd--;
                                        sameEntry = true;
                                        int oldQuantity = customerQuantity[j];
                                        int finalQuantity = oldQuantity + quantity;
                                        if (oldQuantity == 10) //Quantities cannot exceed 10. If quantity is already 10, throw error message
                                        {
                                            Console.WriteLine("You already have 10 of this item. Please pick a new item");
                                            maxQuantity = true;
                                            break;
                                        }
                                        else if (finalQuantity > 10) //Quantities cannot exceed 10. If quantity is already 10, throw error message
                                        {
                                            Console.WriteLine($"You already have {oldQuantity} of this item in your cart. This cannot exceed 10 of this item. \nPlease enter a new quantity:");
                                            failedEntry = true;
                                            break;
                                        }
                                        else //If both the quantites of same item are under 10, update cart and update store inventory lists
                                        {
                                            customerQuantity[j] = customerQuantity[j] + quantity;
                                            decimal newPrice = priceList[itemToAdd] * quantity;
                                            customerPrice[j] = newPrice + customerPrice[j];
                                            inStockList[itemToAdd] = inStockList[itemToAdd] - quantity;
                                            break;
                                        }
                                    }
                                    itemToAdd--;
                                }

                                if (sameEntry == false && failedEntry == false) //If the item selected is not already in the customers cart, updates customer cart and store inventory lists
                                {
                                    customerItemID.Add(itemIDList[itemToAdd]);
                                    customerItemName.Add(itemNameList[itemToAdd]);
                                    customerQuantity.Add(quantity);
                                    customerPrice.Add(priceList[itemToAdd] * quantity);
                                    inStockList[itemToAdd] = inStockList[itemToAdd] - quantity;
                                    break;
                                }
                                else if (maxQuantity == true) //If customer already has 10 of selected item in cart, break the loop
                                    break;
                                else if (failedEntry == false) //If the customers current entered quantity and the quantity in their cart exceeds 10, break the loop
                                    break;
                            }
                            else if (validQuantity == true && quantity > inStockList[itemToAdd]) //If customer tries to add more than what store has in stock
                                Console.WriteLine("This store does not have enough in stock for that quantity. \nPlease enter a new quantity:");
                            else if (validQuantity == true && quantity < 1) //If customer enters a quantity below 0
                                Console.WriteLine("You cannot have a quantity less than 1. \nPlease enter a new quantity:");
                            else //If quantity exceeds 10 items
                                Console.WriteLine("Sorry, quantities are limited to 10 per item. \nPlease enter a new quantity.");
                        }
                        break;
                    }
                    else if (validEntry == true && itemToAdd < -1 || itemToAdd >= itemIDList.Count) //Item ID entered by customer is not valid
                        Console.WriteLine("Invalid item ID. \nPlease enter a new item ID:");
                    else if (validEntry == true && itemToAdd > -1 && itemToAdd < itemIDList.Count && inStockList[itemToAdd] == 0) //If the item customer selected is out of stock
                        Console.WriteLine("We are currently out of this item. \nPlease enter a new item ID:");
                    else if (validEntry == true && itemToAdd == -1) //If customer wishes to enter cart:
                    {
                        if (customerItemID.Count == 0) //Customer cannot view cart if it is empty
                            Console.WriteLine("Your cart is empty. Enter a valid Item ID to create a cart:");
                        else
                        {
                            ViewCustomerCart(); //View customer cart if there is at least 1 item in cart.
                            break;
                        }
                    }
                    else if (adding == "EXIT") //If customer wishes to exit, clear all lists and return to customer main menu
                    {
                        itemIDList.Clear();
                        itemNameList.Clear();
                        inStockList.Clear();
                        priceList.Clear();
                        customerItemID.Clear();
                        customerItemName.Clear();
                        customerQuantity.Clear();
                        customerPrice.Clear();
                        exit = true;
                        break;
                    }
                    else //If there is some unknown error
                        Console.WriteLine("There was an error. \nPlease enter a new item ID:");
                }
            }
        }

        /// <summary>
        /// Customers can remove items from their cart.
        /// Lists are updated accordingly. (Store inventory and customer cart)
        /// </summary>
        public static void RemoveFromCart()
        {
            while (exit == false)
            {
                if (customerItemID.Count == 0) //If customer rmeoves all items from their cart
                {
                    Console.WriteLine("Your cart is now empty.");
                    break;
                }
                //Print off lists containing cart information with correct formatting
                Console.WriteLine($"Cart for store {storeEntry}: {storeName}");
                Console.WriteLine("=========================================");
                Console.WriteLine("Item ID\t Item Name\t Quantity\t Price");
                Console.WriteLine("=======\t =========\t ========\t =====");
                for (int i = 0; i < customerItemID.Count; i++) //Loop through customer cart lists
                {
                    string price = String.Format("{0:0.00}", customerPrice[i]);
                    Console.WriteLine(String.Format("{0, -8} {1, -15} {2, -15} {3, -16}",
                        customerItemID[i], customerItemName[i], customerQuantity[i], $"${price}"));
                }
                Console.WriteLine("=========================");
                Console.WriteLine("Enter an Item ID to remove it from your cart, or enter 0 to view your cart.");
                Console.WriteLine("Type 'EXIT' to cancel your order and return to your account menu.");
                while (exit == false)
                {
                    string? removing = Console.ReadLine();
                    bool validEntry = int.TryParse(removing, out int itemToRemove);
                    itemToRemove--;
                    if (validEntry == true && itemToRemove > -1) //If the Item ID entered is valid
                    {
                        bool failedEntry = true;
                        Console.WriteLine("Enter a quantity to remove from your cart:");
                        while (failedEntry == true)
                        {
                            string? quantityString = Console.ReadLine();
                            int quantity;
                            bool validQuantity = int.TryParse(quantityString, out quantity);
                            if (validQuantity == true && quantity > 0 && quantity < 11) //If quantity entered is valid
                            {
                                itemToRemove++;
                                int itemRemoved = 0;
                                for (int j = 0; j < customerItemID.Count; j++) //Loop through cart to match item ID entered with items in cart
                                {
                                    if (itemToRemove == customerItemID[j]) //If we validate item entered is in the customers cart
                                    {
                                        itemToRemove--;
                                        int oldQuantity = customerQuantity[j];
                                        int finalQuantity = oldQuantity - quantity;
                                        if (finalQuantity < 0) //If customer removes more thna what they have in their cart
                                        {
                                            Console.WriteLine("You cannot remove more than what you have. Please enter a new quantity:");
                                            break;
                                        }
                                        else if (finalQuantity == 0) //If customer removes all quantites of an item from cart, delete item line in lists
                                        {
                                            customerItemID.RemoveAt(j);
                                            customerItemName.RemoveAt(j);
                                            customerQuantity.RemoveAt(j);
                                            customerPrice.RemoveAt(j);
                                            inStockList[itemToRemove] = inStockList[itemToRemove] + quantity;
                                            break;
                                        }
                                        else //If customer only removes part of an items quantites, updates lists accordingly
                                        {
                                            decimal itemPrice = customerPrice[j] / customerQuantity[j];
                                            customerQuantity[j] = customerQuantity[j] - quantity;
                                            decimal newPrice = itemPrice * customerQuantity[j];
                                            customerPrice[j] = newPrice;
                                            inStockList[itemToRemove] = inStockList[itemToRemove] + quantity;
                                            break;
                                        }
                                    }
                                    else
                                        itemRemoved++;
                                }
                                if (itemRemoved == customerItemID.Count) //Item entered is not in customers cart
                                    Console.WriteLine("We could not find that item. Please try again.");
                                break;
                            }
                            else if (validQuantity == true && quantity <= 0) //Quantity entered cannot be less than 1
                                Console.WriteLine("You cannot remove a quantity less than 1. \nPlease enter a new quantity to remove:");
                            else if (validQuantity == true && quantity > 10) //Quantity entered cannot be more than 10
                                Console.WriteLine("You cannot remove a quantity more than 10. \nPlease enter a new quantity to remove:");
                            else //If there is an unknow error
                                Console.WriteLine("There was an error. \nPlease enter a new quantity to remove:");
                        }
                        break;
                    }
                    else if (validEntry == true && itemToRemove == -1) //If customer enters 0 to view cart
                        ViewCustomerCart(); //Return to cart
                    else if (removing == "EXIT") //If customer wishes to exit, clear all lists and return to customer main menu
                    {
                        itemIDList.Clear();
                        itemNameList.Clear();
                        inStockList.Clear();
                        priceList.Clear();
                        customerItemID.Clear();
                        customerItemName.Clear();
                        customerQuantity.Clear();
                        customerPrice.Clear();
                        exit = true;
                        break;
                    }
                    else //If there is some unknown error
                        Console.WriteLine("There was an error. \nPlease enter a new item ID:");
                }
            }
        }

        /// <summary>
        /// Our databases get updated.
        /// Store inventory is altered based on what the customer took
        /// The details of the transaction (customer ID, transaction ID, date and time) are added to the databased
        /// The items in the customer's cart are stored and logged in a database as part of the customer's trasaction
        /// </summary>
        public static void FinalizeTransaction()
        {
            StringBuilder createTransID = new StringBuilder();
            Enumerable
               .Range(65, 26)
               .Select(e => ((char)e).ToString())
               .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
               .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
               .OrderBy(e => Guid.NewGuid())
               .Take(11)
               .ToList().ForEach(e => createTransID.Append(e));
            string transID = createTransID.ToString();

            try
            {
                SpiceItUpService service = new SpiceItUpService(SpiceItUp.Program.server);

                for (int i = 0; i < itemIDList.Count; i++) //Loop through remaining store inventory
                {
                    service.PostNewStoreInventory(inStockList[i], storeEntry, itemIDList[i]);
                }

                service.PostNewTransaction(transID, userID, storeEntry);

                for (int i = 0; i < customerItemID.Count; i++) //Loop through customer cart
                {
                    service.PostNewTransactionDetails(transID, customerItemID[i], customerQuantity[i], customerPrice[i]);
                }

                Console.WriteLine("Your order was successful!");
                Console.WriteLine($"Your transaction ID number is: {transID}");

                //Clear all lists from next run through
                itemIDList.Clear();
                itemNameList.Clear();
                inStockList.Clear();
                priceList.Clear();
                customerItemID.Clear();
                customerItemName.Clear();
                customerQuantity.Clear();
                customerPrice.Clear();
                exit = true;
            }
            catch (Exception) //If there was an error while writing to the databases
            {
                Console.WriteLine("There was an error processing your order. Our stock may have changed.");
            }
        }

        /// <summary>
        /// Customer can view what items they have added to their cart
        /// They are given the option to add items, remove items, checkout, or exit.
        /// </summary>
        public static void ViewCustomerCart()
        {
            //Format the customers cart
            Console.WriteLine($"Cart for store {storeEntry}: {storeName}");
            Console.WriteLine("=========================================");
            Console.WriteLine("Item ID\t Item Name\t Quantity\t Price");
            Console.WriteLine("=======\t =========\t ========\t =====");
            decimal totalPrice = 0;
            for (int i = 0; i < customerItemID.Count; i++) //Loop through the customer's cart lists and print off data
            {
                string price = String.Format("{0:0.00}", customerPrice[i]);
                Console.WriteLine(String.Format("{0, -8} {1, -15} {2, -15} {3, -16}",
                    customerItemID[i], customerItemName[i], customerQuantity[i], $"${price}"));
                totalPrice = totalPrice + customerPrice[i];
            }
            //Print off total price of entire cart
            string stringTotalPrice = String.Format("{0:0.00}", totalPrice);
            Console.WriteLine("=========================");
            Console.WriteLine($"Your total is ${stringTotalPrice}");
            Console.WriteLine("=========================");
            Console.WriteLine("How would you like to continue?");
            Console.WriteLine("1: Add more items to my cart \n2: Remove items from my cart \n3: Finalize transaction and checkout \n4: Cancel transaction and return to menu");

            int userEntry;
            while (true) //Test to ensure user entry is valid
            {
                string? mySelection = Console.ReadLine();
                bool validEntry = int.TryParse(mySelection, out userEntry);
                if (validEntry == true && userEntry >= 1 && userEntry <= 4)
                {
                    if (userEntry == 3 && customerItemID.Count == 0)
                        Console.WriteLine("You cannot checkout with an empty cart. Please select another option:");
                    else
                        break; //Break when valid
                }
                else
                    Console.WriteLine("Invalid selection. Please try again.");
            }

            switch (userEntry)
            {
                case 1:
                    AddToCart(); //Add more items to customers cart
                    break;
                case 2:
                    RemoveFromCart(); //Remove items from customers cart
                    break;
                case 3:
                    Console.WriteLine("Are you sure? (Y/N)"); //Ensure customer is ready to checkout, otherwise, cancel
                    string? check = Console.ReadLine();
                    if ("Y" == check?.ToUpper())
                        FinalizeTransaction();
                    break;
                case 4: //If customer exits, clear all lists
                    itemIDList.Clear();
                    itemNameList.Clear();
                    inStockList.Clear();
                    priceList.Clear();
                    customerItemID.Clear();
                    customerItemName.Clear();
                    customerQuantity.Clear();
                    customerPrice.Clear();
                    exit = true;
                    break;
            }
        }
    }
}
