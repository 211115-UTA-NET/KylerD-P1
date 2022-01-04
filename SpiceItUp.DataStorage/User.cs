using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class User
    {
        public int id {get;}
        public string first { get; }
        public string last { get; }
        public double phone { get; }
        public string employee { get; }

        public User()
        {

        }

        public User(int id, string first, string last, double phone, string employee)
        {
            this.id = id;
            this.first = first;
            this.last = last;
            this.phone = phone;
            this.employee = employee;
        }

        public User(int id, string first, string last, double phone)
        {
            this.id = id;
            this.first = first;
            this.last = last;
            this.phone = phone;
        }
    }
}
