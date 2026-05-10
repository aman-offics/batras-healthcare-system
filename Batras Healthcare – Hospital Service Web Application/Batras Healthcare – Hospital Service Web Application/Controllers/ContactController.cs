using Batras_Healthcare___Hospital_Service_Web_Application.Models;
using Microsoft.AspNetCore.Mvc;


namespace Batras_Healthcare___Hospital_Service_Web_Application.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class ContactController : ControllerBase
        {
            private readonly AppDbContext _context;

            public ContactController(AppDbContext context)
            {
                _context = context;
            }

            [HttpPost("send")]
            public IActionResult Send(ContactMessage msg)
            {
                _context.ContactMessages.Add(msg);
                _context.SaveChanges();
                return Ok();
            }

            [HttpGet("all")]
            public IActionResult GetAll()
            {
                return Ok(_context.ContactMessages.ToList());
            }
        }
    
}
