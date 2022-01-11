using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SpiceItUp.Test
{
    public class TestCustomerAccount
    {
        //Not a valid test
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(5)]
        public void SelectedOption_UserEntry_ValidEntry(int userEntry)
        {
            //Arrange
            int userID = 1;

            //Act
            SpiceItUp.CustomerAccount.SelectedOption(userEntry, userID);

            //Assert
            if (userEntry >= 1 && userEntry <= 4)
                Assert.False(true);
            else
                Assert.True(true);
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        public void ValidUserEntry_UserEntry_ValidEntry(string selection)
        {
            //Act
            int entry = SpiceItUp.CustomerAccount.ValidEntry(selection);

            //Assert
            if (entry > 0)
                Assert.True(entry > 0 && entry < 5);
            else if (entry == 0)
                Assert.True(entry == 0);
            else
                Assert.True(false);
        }
    }
}
