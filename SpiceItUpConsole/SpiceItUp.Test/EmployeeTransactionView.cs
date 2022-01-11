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
    public class TestLocationInventory
    {
        [Theory]
        [InlineData("99")]
        [InlineData("100")]
        [InlineData("101")]
        [InlineData("102")]
        [InlineData("103")]
        [InlineData("104")]
        [InlineData("105")]
        public void ValidStore_StireEntry_ValidEntry(string selection)
        {
            //Act
            int entry = SpiceItUp.LocationInventory.ValidStore(selection);

            //Assert
            if (entry > 100 && entry < 105)
                Assert.True(entry > 100 && entry < 105);
            else if (entry == 0)
                Assert.True(entry == 0);
            else
                Assert.True(false);
        }
    }
}
