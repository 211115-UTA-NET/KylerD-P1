using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp.Dtos
{
    public class Store
    {
        public int StoreID { get; set; }
        public string? StoreName { get; set; }
        public string? ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public string? ItemPrice { get; set; }
    }
}
