using Xunit;
using SpiceItUpDataStorage;
using System.Collections.Generic;
using Moq;
using System.Threading.Tasks;
using SpiceItUp.Api.Controllers;

namespace SpiceItUpAPI.Test
{
    public static class MoqTest
    {
        [Fact]
        public static async Task StoreListTest()
        {
            //Arrange
            List<Store> storeList = new()
            {
                new()
                {
                    StoreID = 101,
                    StoreName = "Spice It Up Chattanooga"
                },
                new()
                {
                    StoreID = 102,
                    StoreName = "Spice It Up Knoxville"
                },
                new()
                {
                    StoreID = 103,
                    StoreName = "Spice It Up Nashville"
                },
                new()
                {
                    StoreID = 104,
                    StoreName = "Spice It Up Memphis"
                }
            };

            var moq = new Mock<IRepository>();
            moq.Setup(x => x.GetStoreList()).Returns(await Task.FromResult(storeList));
            var controller = new StoreController(moq.Object);

            //Act
            var expected = controller.StoreList();

            //Assert
            for (int i = 0; i < storeList.Count; i++)
            {
                Assert.Equal(storeList[i].StoreID, (expected)[i].StoreID);
                Assert.Equal(storeList[i].StoreName, (expected)[i].StoreName);
            }
        }
    }
}