using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiceItUpDataStorage;

namespace SpiceItUp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {

        private readonly IRepository _repository;
        public StoreController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public List<Store> StoreList()
        {
            IEnumerable<Store> storeList = _repository.GetStoreList();
            return storeList.ToList();
        }

        // GET api/store
        [HttpGet("/store")]
        public IActionResult GetStoreList()
        {
            IEnumerable<Store> store;

            try
            {
                store = SqlRepository.PrintStoreList();
            }
            catch(Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(store);
        }

        // GET api/store/inventory?id=storeID
        [HttpGet("/store/inventory")]
        public IActionResult GetStoreInventory([FromQuery] int storeID)
        {
            IEnumerable<Store> store;

            try
            {
                store = SqlRepository.PrintStoreInventory(storeID);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(store);
        }

        // GET api/storeinfo/id=storeID
        [HttpGet("/storeinfo")]
        public IActionResult GetStoreInfo([FromQuery] int storeID)
        {
            IEnumerable<Store> store;

            try
            {
                store = SqlRepository.GetStoreInfo(storeID);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(store);
        }

        // GET api/storeinfo/itemid/StoreID=storeID
        [HttpGet("/storecart")]
        public IActionResult GetCartStoreInv([FromQuery] int storeID)
        {
            IEnumerable<Store> store;

            try
            {
                store = SqlRepository.GetCartStoreInventory(storeID);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(store);
        }

        // POST api/newinventory
        [HttpPost("/newinventory")]
        public IActionResult PostNewStoreInventroy([FromQuery] int inStockListNew, int storeEntry, int itemIDListNew)
        {
            try
            {
                SqlRepository.NewStoreInventory(inStockListNew, storeEntry, itemIDListNew);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return StatusCode(200);
        }
    }
}
