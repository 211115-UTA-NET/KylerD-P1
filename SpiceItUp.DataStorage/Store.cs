using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class Store
    {
        public int StoreID { get; }
        public string? StoreName { get; }
        public string? ItemName { get; }
        public int ItemQuantity { get; }
        public string? ItemPrice { get; }

        public Store()
        {

        }

        public Store(int storeID, string storeName)
        {
            this.StoreID = storeID;
            this.StoreName = storeName;
        }

        public Store(string itemName, int itemQuantity, string itemPrice)
        {
            this.ItemName = itemName;
            this.ItemQuantity = itemQuantity;
            this.ItemPrice = itemPrice;
        }
    }
}
