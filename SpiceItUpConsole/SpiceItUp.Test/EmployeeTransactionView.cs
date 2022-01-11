using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace SpiceItUp.Test
{
    /// <summary>
    /// We should not be able to view transaction details if an incorrect store is entered
    /// </summary>
    public class EmployeeTransactionView
    {
        ///// <summary>
        ///// If we select an incorrect store, we can't pull records
        ///// </summary>
        //[Fact]
        //public async void StoreTransactionHistory_UserID_InvalidEntry()
        //{
        //    //Arrange
        //    int store = 100;

        //    //Act
        //    //trans = (List<Transaction>)SqlRepository.StoreTransactionHistory(store);


        //    //Assert
        //    Assert.True(trans.Count == 0);

        //    //Arrange
        //    SpiceItUpService service = new SpiceItUpService(SpiceItUp.Program.server);
        //    int entry = 100;

        //    //Act
        //    List<Transaction> trans = await service.GetStoreTransactionList(entry);
        //    _ = EmployeeTransactionByStore.TransactionHistory(100);

        //   //Assert
        //   Assert.False(EmployeeTransactionByStore.transList.Count == 0);
        //}

        /////// <summary>
        /////// If we select an incorrect store, we can't pull records
        /////// </summary>
        //[Fact]
        //public void CustomerNameTransactionHistory_UserID_InvalidEntry()
        //{
        //    //Act
        //    EmployeeTransactionByCustomer.TransactionHistory(100);

        //    //Assert
        //    Assert.False(EmployeeTransactionByCustomer.transList.Count == 0);
        //}

        //[Fact]
        //public static void StoreTransactionHistory_UserID_InvalidEntry()
        //{
        //    //Arrange
        //    List<Transaction> trans = new();
        //    int store = 100;

        //    //Act
        //    trans = (List<Transaction>)SqlRepository.StoreTransactionHistory(store);


        //    //Assert
        //    Assert.True(trans.Count == 0);
        //}

        //[Fact]
        //public static void StoreTransactionHistory_UserID_ValidEntry()
        //{
        //    //Arrange
        //    List<Transaction> trans = new();
        //    int store = 104;

        //    //Act
        //    trans = (List<Transaction>)SqlRepository.StoreTransactionHistory(store);


        //    //Assert
        //    Assert.True(trans.Count > 0);
        //}
    }
}
