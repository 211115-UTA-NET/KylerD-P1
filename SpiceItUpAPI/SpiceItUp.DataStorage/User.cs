using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class User
    {
        public int Id {get;}
        public string? First { get; }
        public string? Last { get; }
        public double Phone { get; }
        public string? Employee { get; }
        public string? Username { get; }
        public string? Password { get; }
        public string? PhoneNumber { get; }

        public User()
        {

        }

        public User(string username, string password, string firstName, string lastName, string phoneNumber)
        {
            this.Username = username;
            this.Password = password;
            this.First = firstName;
            this.Last = lastName;
            this.PhoneNumber = phoneNumber;
        }

        public User(int id, string first, string last, double phone, string employee)
        {
            this.Id = id;
            this.First = first;
            this.Last = last;
            this.Phone = phone;
            this.Employee = employee;
        }

        public User(int id, string first, string last, double phone)
        {
            this.Id = id;
            this.First = first;
            this.Last = last;
            this.Phone = phone;
        }

        public User(int id, string username, string password)
        {
            this.Id= id;
            this.Username = username;
            this.Password = password;
        }

        public User(int id)
        {
            this.Id = id;
        }
    }
}
