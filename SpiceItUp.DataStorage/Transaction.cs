using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class Transaction
    {
        public string? TransID { get; }
        public int StoreID { get; }
        public string? Price { get; }
        public string? StoreName { get; }
        public string? Timestamp { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public string? ItemName { get; }
        public int Quantity { get; }
        public string? ItemPrice { get; }
        public string? TotalPrice { get; }

        public Transaction()
        {

        }

        public Transaction(string transID, int storeID, string price)
        {
            this.TransID = transID;
            this.StoreID = storeID;
            this.Price = price;
        }

        public Transaction(string transID, int storeID, string storeName, string timestamp, string firstName, string lastName, string totalPrice, string itemName, int quantity, string itemPrice)
        {
            this.TransID = transID;
            this.StoreID= storeID;
            this.StoreName = storeName;
            this.Timestamp = timestamp;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.TotalPrice = totalPrice;
            this.ItemName = itemName;
            this.Quantity = quantity;
            this.ItemPrice = itemPrice;
        }
    }
}
