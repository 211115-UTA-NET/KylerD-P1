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
        private readonly IRepository _repo;

        public UserController(IRepository repository)
        {
            _repo = repository;
        }

        // GET api/user/?firstName={FirstName}
        [HttpGet]
        public IActionResult GetAllUsersFirstName([FromQuery, Required] string firstName)
        {
            IEnumerable<User> user;
            
            user = _repo.SearchCustomerFirstName(firstName);

            return new JsonResult(user);
        }
    }
}
