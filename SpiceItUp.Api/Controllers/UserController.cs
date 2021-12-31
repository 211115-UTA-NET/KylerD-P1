using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiceItUpDataStorage;
using System.ComponentModel.DataAnnotations;

namespace SpiceItUpDataStorage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly SpiceItUpDataStorage _repository;

        //public UserController(SqlRepository repository)
        //{
        //    _repository = repository;
        //}


        [HttpGet]
        public IActionResult GetAllUsers([FromQuery, Required] string firstName)
        {
            SpiceItUpDataStorage.SqlRepository.SearchCustomerFirstName(firstName);
            return new JsonResult(User);
        }
    }
}
