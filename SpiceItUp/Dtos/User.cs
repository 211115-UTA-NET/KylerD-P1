using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp.Dtos
{
    public class User
    {
        public int Id { get; set; }
        public string? First { get; set; }
        public string? Last { get; set; }
        public double Phone { get; set; }
        public string? Employee { get; set; }
    }
}
