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
    }
}