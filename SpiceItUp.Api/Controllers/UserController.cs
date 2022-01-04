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

        // GET api/user
        [HttpGet("/user")]
        public IActionResult GetAllCustomerInfo()
        {
            IEnumerable<User> user;

            user = _repo.CustomerList();

            return new JsonResult(user);
        }

        // GET api/user/?firstName={FirstName}
        [HttpGet("/user/FirstName")]
        public IActionResult GetAllUsersFirstName([FromQuery] string firstName)
        {
            IEnumerable<User> user;
            
            user = _repo.SearchCustomerFirstName(firstName);

            return new JsonResult(user);
        }

        // GET api/user/?lastName={LastName}
        [HttpGet("/user/LastName")]
        public IActionResult GetAllUsersLastName([FromQuery] string lastName)
        {
            IEnumerable<User> user;

            user = _repo.SearchCustomerLastName(lastName);

            return new JsonResult(user);
        }
    }
}
