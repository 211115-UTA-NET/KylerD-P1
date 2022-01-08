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
        private readonly SqlRepository _repo;

        public UserController(SqlRepository repository)
        {
            _repo = repository;
        }

        // GET api/user
        [HttpGet("/user")]
        public IActionResult GetAllCustomerInfo()
        {
            IEnumerable<User> user;

            try
            {
                user = _repo.CustomerList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(user);
        }

        // GET api/user/?firstName={FirstName}
        [HttpGet("/user/FirstName")]
        public IActionResult GetAllUsersFirstName([FromQuery] string firstName)
        {
            IEnumerable<User> user;
            
            try
            {
                user = _repo.SearchCustomerFirstName(firstName);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(user);
        }

        // GET api/user/?lastName={LastName}
        [HttpGet("/user/LastName")]
        public IActionResult GetAllUsersLastName([FromQuery] string lastName)
        {
            IEnumerable<User> user;

            try
            {
                user = _repo.SearchCustomerLastName(lastName);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(user);
        }

        // GET api/user/?lastName={LastName}
        [HttpGet("/user/Login")]
        public IActionResult GetUserID([FromQuery] string username, string password)
        {
            IEnumerable<User> user;

            try
            {
                user = _repo.GetLoginUserID(username, password);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(user);
        }

        // GET api/user/?userID={userID}
        [HttpGet("/user/ID")]
        public IActionResult GetCustomerInfo([FromQuery] int id)
        {
            IEnumerable<User> user;
            try
            {
                user = _repo.GetCustomerInfo(id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return new JsonResult(user);
        }

        // POST api/user
        [HttpPost("/newuser")]
        public IActionResult PostNewCustomer([FromQuery] string username, string password, string firstName, string lastName, string phoneNumber, SqlRepository repository)
        {
            try
            {
                repository.PostCustomerInfo(username, password, firstName, lastName, phoneNumber);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return StatusCode(200);
        }
    }
}
