using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiceItUpDataStorage;
using System.ComponentModel.DataAnnotations;

namespace SpiceItUp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IRepository _repo;

        public TransactionController(IRepository repository)
        {
            _repo = repository;
        }

        // GET transaction/customer={id}
        [HttpGet("/transaction/customer")]
        public IActionResult GetCustomerTransactions([FromQuery] int id)
        {
            IEnumerable<Transaction> trans;

            try
            {
                trans = _repo.CustomerTransactionHistory(id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(trans);
        }

        // GET transaction/storeID={storeID}
        [HttpGet("/transaction/storeID")]
        public IActionResult GetStoreTransactions([FromQuery] int storeID)
        {
            IEnumerable<Transaction> trans;

            try
            {
                trans = _repo.StoreTransactionHistory(storeID);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(trans);
        }

        // GET transaction/transID={transID}
        [HttpGet("/transaction/transID")]
        public IActionResult GetTransactionDetails([FromQuery] string transID)
        {
            IEnumerable<Transaction> trans;

            try
            {
                trans = _repo.DetailedTransaction(transID);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(trans);
        }
    }
}
