
//using HospitalAPI.Models;

//namespace Batras_Healthcare___Hospital_Service_Web_Application.Controllers
//{
//    public class DoctorController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}




using Microsoft.AspNetCore.Mvc;
    namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        // 👉 GET ALL DOCTORS
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var doctors = _context.Doctors.ToList();
            return Ok(doctors);
        }
    }
}