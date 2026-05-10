using Microsoft.AspNetCore.Mvc;
using HospitalAPI.Models;
using Batras_Healthcare___Hospital_Service_Web_Application.Models;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }


        // 🔥 REGISTER
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            user.IsActive = true;
            user.Role = "User"; // 🔥 always set

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }



        // 🔥 LOGIN
        [HttpGet("login")]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Email.ToLower() == email.ToLower() &&
                    x.Password == password);

            if (user == null)
            {
                return BadRequest("Invalid login"); // 🔥 IMPORTANT
            }

            return Ok(user);
        }
        // GET USERS
        [HttpGet("all")]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }


        // DEACTIVATE
        [HttpPut("deactivate/{id}")]
        public IActionResult Deactivate(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            user.IsActive = false;
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("activate/{id}")]
        public IActionResult Activate(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            user.IsActive = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("toggle/{id}")]
        public IActionResult Toggle(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            if (user.Role == "Admin")
                return BadRequest("Cannot modify admin ❌");

            user.IsActive = !user.IsActive; // 🔥 TOGGLE
            _context.SaveChanges();

            return Ok();
        }
    }
}

