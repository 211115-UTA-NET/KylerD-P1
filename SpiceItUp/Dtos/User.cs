using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp.Dtos
{
    public class User
    {
        public int id { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public double phone { get; set; }
        public string employee { get; set; }
    }
}
