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
        // GET transaction/customer={id}
        [HttpGet("/transaction/customer")]
        public IActionResult GetCustomerTransactions([FromQuery] int id)
        {
            IEnumerable<Transaction> trans;

            try
            {
                trans = SqlRepository.CustomerTransactionHistory(id);
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
                trans = SqlRepository.StoreTransactionHistory(storeID);
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
                trans = SqlRepository.DetailedTransaction(transID);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(trans);
        }

        // POST api/newtransaction
        [HttpPost("/newtransaction")]
        public IActionResult PostNewTransaction([FromQuery] string transID, int userID, int storeEntry)
        {
            try
            {
                SqlRepository.NewTransaction(transID, userID, storeEntry);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return StatusCode(200);
        }

        // POST api/newtransactiondetails
        [HttpPost("/newtransactiondetails")]
        public IActionResult PostTransactionDetails([FromQuery] string transID, int customerItemIDNew, int customerQuantityNew, decimal customerPriceNew)
        {
            try
            {
                SqlRepository.TransactionDetails(transID, customerItemIDNew, customerQuantityNew, customerPriceNew);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return StatusCode(200);
        }
    }
}
