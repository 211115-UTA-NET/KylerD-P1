using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp.Dtos
{
    public class Transaction
    {
        public string? TransID { get; set; }
        public int StoreID { get; set; }
        public string? Price { get; set; }
        public string? StoreName { get; set; }
        public string? Timestamp { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public string? ItemPrice { get; set; }
        public string? TotalPrice { get; set; }
    }
}
