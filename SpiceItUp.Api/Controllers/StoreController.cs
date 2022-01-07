using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiceItUpDataStorage;

namespace SpiceItUp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IRepository _repo;

        public StoreController(IRepository repository)
        {
            _repo = repository;
        }

        // GET api/store
        [HttpGet("/store")]
        public IActionResult GetStoreList()
        {
            IEnumerable<Store> store;

            try
            {
                store = _repo.PrintStoreList();
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
                store = _repo.PrintStoreInventory(storeID);
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
                store = _repo.GetStoreInfo(storeID);
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
                store = _repo.GetCartStoreInventory(storeID);
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
                IRepository.NewStoreInventory(inStockListNew, storeEntry, itemIDListNew);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return StatusCode(200);
        }
    }
}
