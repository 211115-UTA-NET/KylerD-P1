using Xunit;

namespace SpiceItUp.Test
{
    /// <summary>
    /// Check if the account exists
    /// Is this user an employee or not?
    /// </summary>
    public class TestEmployeeAccount
    {
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
            int entry = SpiceItUp.EmployeeAccount.ValidEntry(selection);

            //Assert
            if (entry > 0)
                Assert.True(entry > 0 && entry < 6);
            else if (entry == 0)
                Assert.True(entry == 0);
            else
                Assert.True(false);
        }
    }
}