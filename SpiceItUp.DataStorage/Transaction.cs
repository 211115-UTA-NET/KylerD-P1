using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class Transaction
    {
        public string transID { get; }
        public int storeID { get; }
        public string price { get; }

        public Transaction()
        {

        }

        public Transaction(string transID, int storeID, string price)
        {
            this.transID = transID;
            this.storeID = storeID;
            this.price = price;
        }
    }
}
