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

            store = _repo.PrintStoreList();

            return new JsonResult(store);
        }

        // GET api/user/inventory?id=storeID
        [HttpGet("/store/inventory")]
        public IActionResult GetStoreInventory([FromQuery] int storeID)
        {
            IEnumerable<Store> store;

            store = _repo.PrintStoreInventory(storeID);

            return new JsonResult(store);
        }
    }
}
