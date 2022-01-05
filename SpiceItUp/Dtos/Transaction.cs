using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp.Dtos
{
    public class Transaction
    {
        public string transID { get; set; }
        public int storeID { get; set; }
        public string price { get; set; }
    }
}
