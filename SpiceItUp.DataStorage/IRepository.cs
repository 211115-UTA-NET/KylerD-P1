using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class IRepository
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        private static List<int> customerIDList = new List<int>();

        public IEnumerable<User> SearchCustomerFirstName(string firstName)
        {
            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "=======", "=========", "=========", "============", "============"));

            //Pull customer information matching first name entered
            connection.Open();
            string customerSearch = "SELECT UserID, FirstName, LastName, PhoneNumber, IsEmployee FROM UserInformation " +
                "WHERE FirstName = @firstName ORDER BY LastName;";
            using SqlCommand getCustomer = new(customerSearch, connection);
            getCustomer.Parameters.Add("@firstName", System.Data.SqlDbType.VarChar).Value = firstName;
            using SqlDataReader reader = getCustomer.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string first = reader.GetString(1);
                string last = reader.GetString(2);
                double phone = reader.GetInt64(3);
                string employee = reader.GetString(4);
                result.Add(new(id, first, last, phone, employee));

                Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3), reader.GetString(4)));
            }
            Console.WriteLine("==============================");

            return result;
        }

        public IEnumerable<User> SearchCustomerLastName(string lastName)
        {
            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "User ID", "FirstName", "Last Name", "Phone Number", "Is Employee?"));
            Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                    "=======", "=========", "=========", "============", "============"));

            //Pull customer information matching last name entered
            connection.Open();
            string customerSearch = "SELECT UserID, FirstName, LastName, PhoneNumber, IsEmployee FROM UserInformation " +
                "WHERE LastName = @lastName ORDER BY LastName;";
            using SqlCommand getCustomer = new(customerSearch, connection);
            getCustomer.Parameters.Add("@lastName", System.Data.SqlDbType.VarChar).Value = lastName;
            using SqlDataReader reader = getCustomer.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string first = reader.GetString(1);
                string last = reader.GetString(2);
                double phone = reader.GetInt64(3);
                string employee = reader.GetString(4);
                result.Add(new(id, first, last, phone, employee));

                Console.WriteLine(String.Format("{0, -10} {1, -15} {2, -15} {3, -15} {4, -15}",
                       reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3), reader.GetString(4)));
            }
            Console.WriteLine("==============================");

            return result;
        }

        public IEnumerable<User> CustomerList()
        {
            customerIDList.Clear();

            List<User> result = new List<User>();

            using SqlConnection connection = new(connectionString);

            //Format our customer list
            Console.WriteLine("Here is the customer list:");
            Console.WriteLine("==============================");
            Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    "Entry", "First Name", "Last Name", "Phone Number"));
            Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    "=====", "==========", "=========", "============"));

            //Pull a list of customers from the database
            connection.Open();
            string getCustomerList = "SELECT UserID, FirstName, LastName, PhoneNumber FROM UserInformation WHERE IsEmployee = 'FALSE';";
            using SqlCommand readCustomerList = new(getCustomerList, connection);
            using SqlDataReader readCustomers = readCustomerList.ExecuteReader();
            int entry = 1;
            while (readCustomers.Read())
            {
                int id = readCustomers.GetInt32(0);
                string first = readCustomers.GetString(1);
                string last = readCustomers.GetString(2);
                double phone = readCustomers.GetInt64(3);
                result.Add(new(id, first, last, phone));

                customerIDList.Add(readCustomers.GetInt32(0));
                Console.WriteLine(String.Format("{0, -7} {1, -15} {2, -15} {3, -10}",
                    entry, readCustomers.GetString(1), readCustomers.GetString(2), readCustomers.GetInt64(3)));
                entry++;
            }
            connection.Close();

            return result;
        }
    }
}