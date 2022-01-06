using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public class SqlRepository : IRepository
    {
        private static string connectionString = File.ReadAllText("D:/Revature/ConnectionStrings/SpiceItUp-P0-KylerD.txt");

        public static void FinalizeTransaction(List<int> itemIDListNew, List<int> inStockListNew, int storeEntry, string transID, int userID, List<int> customerItemIDNew, List<int> customerQuantityNew, List<decimal> customerPriceNew)
        {
            using SqlConnection connection = new(connectionString);

            for (int i = 0; i < itemIDListNew.Count; i++) //Loop through remaining store inventory
            {
                //Updates store quantites based on what customer has added to their cart
                connection.Open();
                string updateStoreInv = "UPDATE StoreInventory SET InStock = @stock WHERE StoreID = @storeID AND ItemID = @itemID;";
                using SqlCommand newStoreInv = new(updateStoreInv, connection);
                newStoreInv.Parameters.Add("@stock", System.Data.SqlDbType.Int).Value = inStockListNew[i];
                newStoreInv.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
                newStoreInv.Parameters.Add("@itemID", System.Data.SqlDbType.Int).Value = itemIDListNew[i];
                newStoreInv.ExecuteNonQuery();
                connection.Close();
            }

            //Add details of transaction to database
            connection.Open();
            string addTransHistory = "INSERT TransactionHistory (TransactionID, UserID, StoreID, IsStoreOrder, Timestamp) " +
                "VALUES (@transID, @userID, @storeID, @isStoreOrder, @timestamp);";
            using SqlCommand newTransHistory = new(addTransHistory, connection);
            DateTime now = DateTime.Now;
            string dateTime = now.ToString("F");
            newTransHistory.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = transID;
            newTransHistory.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = userID;
            newTransHistory.Parameters.Add("@storeID", System.Data.SqlDbType.Int).Value = storeEntry;
            newTransHistory.Parameters.Add("@isStoreOrder", System.Data.SqlDbType.VarChar).Value = "FALSE";
            newTransHistory.Parameters.Add("@timestamp", System.Data.SqlDbType.NVarChar).Value = dateTime;
            newTransHistory.ExecuteNonQuery();
            connection.Close();

            for (int i = 0; i < customerItemIDNew.Count; i++) //Loop through customer cart
            {
                //Add customer cart items to database
                connection.Open();
                string addTransDetails = "INSERT CustomerTransactionDetails (TransactionID, ItemID, Quantity, Price) " +
                    "VALUES (@transID, @itemID, @quantity, @price);";
                using SqlCommand newTransDetails = new(addTransDetails, connection);
                newTransDetails.Parameters.Add("@transID", System.Data.SqlDbType.VarChar).Value = transID;
                newTransDetails.Parameters.Add("@itemID", System.Data.SqlDbType.Int).Value = customerItemIDNew[i];
                newTransDetails.Parameters.Add("@quantity", System.Data.SqlDbType.Int).Value = customerQuantityNew[i];
                newTransDetails.Parameters.Add("@price", System.Data.SqlDbType.Money).Value = customerPriceNew[i];
                newTransDetails.ExecuteNonQuery();
                connection.Close();
            }

            //Clear all lists from next run through
            itemIDListNew.Clear();
            inStockListNew.Clear();
            customerItemIDNew.Clear();
            customerQuantityNew.Clear();
            customerPriceNew.Clear();
        }
    }
}
