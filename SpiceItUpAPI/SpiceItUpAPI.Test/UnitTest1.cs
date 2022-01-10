using Xunit;
using SpiceItUpDataStorage;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SpiceItUpAPI.Test
{
    public static class UnitTest1
    {
        //private static string connectionString = "Server=tcp:spiceitup-p0-kylerd.database.windows.net,1433;Initial Catalog=SpiceItUp-P0-KylerD;Persist Security Info=False;User ID=LetsGetSpicy;Password=P0StoreEmulator!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        [Fact]
        public static void StoreTransactionHistory_UserID_InvalidEntry()
        {
            //Arrange
            List<Transaction> trans = new();
            int store = 100;

            //Act
            trans = (List<Transaction>)SqlRepository.StoreTransactionHistory(store);
            

            //Assert
            Assert.True(trans.Count == 0);
        }

        [Fact]
        public static void StoreTransactionHistory_UserID_ValidEntry()
        {
            //Arrange
            List<Transaction> trans = new();
            int store = 104;

            //Act
            trans = (List<Transaction>)SqlRepository.StoreTransactionHistory(store);


            //Assert
            Assert.True(trans.Count > 0);
        }

        //[Fact]
        //public void CustomerNameTransactionHistory_UserID_InvalidEntry()
        //{
        //    //Act
        //    EmployeeTransactionByCustomer.TransactionHistory(100);

        //    //Assert
        //    Assert.False(EmployeeTransactionByCustomer.transList.Count == 0);
        //}

        //[Fact]
        //public void TestEntries_UsernamePassword_ValidAccount()
        //{
        //    //Arrange
        //    string username = "ManagerKyler";
        //    string password = "ManagerPassword";
        //    string firstNameResult = "Kyler";
        //    string lastNameResult = "Dennis";

        //    //Act
        //    AccountLogin.TestEntries(username, password);

        //    //Assert
        //    Assert.True(firstNameResult.Equals(AccountLogin.firstName) && lastNameResult.Equals(AccountLogin.lastName));
        //}

        //[Fact]
        //public void TestEntries_UsernamePassword_EmployeeTrue()
        //{
        //    //Arrange
        //    string username = "ManagerKyler";
        //    string password = "ManagerPassword";
        //    string isEmployeeResult = "TRUE";

        //    //Act
        //    AccountLogin.TestEntries(username, password);

        //    //Assert
        //    Assert.True(isEmployeeResult.Equals(AccountLogin.isEmployee));
        //}

        //[Fact]
        //public void TestEntries_UsernamePassword_EmployeeFalse()
        //{
        //    Arrange
        //    string username = "Daniel0";
        //    string password = "Everett0";
        //    string isEmployeeResult = "FALSE";

        //    //Act
        //    AccountLogin.TestEntries(username, password);

        //    //Assert
        //    Assert.True(isEmployeeResult.Equals(AccountLogin.isEmployee));
        //}

        //[Fact]
        //public void CustmerLookup_FirstName_ValidCustomer()
        //{
        //    //Arrange
        //    string firstName = "Daniel";
        //    CustomerLookup.firstName = firstName;

        //    //Act
        //    CustomerLookup.SearchByFirstName();

        //    //Assert
        //    Assert.True(CustomerLookup.test >= 0);
        //}

        //[Fact]
        //public void CustmerLookup_FirstName_InValidCustomer()
        //{
        //    //Arrange
        //    string firstName = "Greg";
        //    CustomerLookup.firstName = firstName;

        //    //Act
        //    CustomerLookup.SearchByFirstName();

        //    //Assert
        //    Assert.False(CustomerLookup.test >= 0);
        //}

        //[Fact]
        //public void CustmerLookup_LastName_ValidCustomer()
        //{
        //    //Arrange
        //    string lastName = "Wooten";
        //    CustomerLookup.lastName = lastName;

        //    //Act
        //    CustomerLookup.SearchByLastName();

        //    //Assert
        //    Assert.True(CustomerLookup.test >= 0);
        //}

        //[Fact]
        //public void CustmerLookup_LastName_InValidCustomer()
        //{
        //    //Arrange
        //    string lastName = "Failue";
        //    CustomerLookup.lastName = lastName;

        //    //Act
        //    CustomerLookup.SearchByLastName();

        //    //Assert
        //    Assert.False(CustomerLookup.test >= 0);
        //}

        //[Fact]
        //public void CustomerTransactionHistory_UserID_InvalidUser()
        //{
        //    //Act
        //    CustomerOrderHistory.CustomerTransactionHistory(0);

        //    //Assert
        //    Assert.True(CustomerOrderHistory.transList.Count == 0);
        //}
    }
}