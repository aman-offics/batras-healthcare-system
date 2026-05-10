using Microsoft.AspNetCore.Mvc;
using Batras_Healthcare___Hospital_Service_Web_Application.Models;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public AppointmentController(AppDbContext context)
    {
        _context = context;
    }



    [HttpPost("book")]
    public IActionResult Book(Appointment app)
    {
        var exists = _context.Appointments.Any(x =>
            x.DoctorName == app.DoctorName &&
            x.AppointmentDate == app.AppointmentDate &&
            x.AppointmentTime == app.AppointmentTime
        );

        if (exists)
        {
            return BadRequest("This time slot is already booked!");
        }

        app.Status = "Pending";

        _context.Appointments.Add(app);
        _context.SaveChanges();

        return Ok(app);
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        var data = _context.Appointments.ToList();
        return Ok(data);
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.Appointments.Find(id);

        if (item == null)
            return NotFound();

        _context.Appointments.Remove(item);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPut("status/{id}")]
    public IActionResult UpdateStatus(int id, string status)
    {
        var item = _context.Appointments.Find(id);

        if (item == null)
            return NotFound();

        item.Status = status;
        _context.SaveChanges();

        return Ok();
    }

    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }

    [HttpGet("byemail/{email}")]
    public IActionResult GetByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("Email is required");

        var data = _context.Appointments
            .Where(x => x.Email.ToLower() == email.ToLower())
            .ToList();

        return Ok(data);
    }

    [HttpGet("slots")]
    public IActionResult GetBookedSlots(string doctor, DateTime date)
    {
        var slots = _context.Appointments
            .Where(x => x.DoctorName == doctor && x.AppointmentDate == date)
            .Select(x => x.AppointmentTime)
            .ToList();

        return Ok(slots);
    }




    [HttpGet("alldoctors")]
    public IActionResult GetDoctors()
    {
        return Ok(_context.Doctors.ToList());
    }

    [HttpPost("adddoctor")]
    public IActionResult AddDoctor(Doctor doc)
    {
        _context.Doctors.Add(doc);
        _context.SaveChanges();
        return Ok();
    }

    [HttpDelete("deletedoctor/{id}")]
    public IActionResult DeleteDoctor(int id)
    {
        var doc = _context.Doctors.Find(id);
        if (doc == null) return NotFound();

        _context.Doctors.Remove(doc);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPut("updatedoctor")]
    public IActionResult UpdateDoctor(Doctor doc)
    {
        var existing = _context.Doctors.Find(doc.Id);

        if (existing == null)
            return NotFound();

        existing.Name = doc.Name;
        existing.Specialization = doc.Specialization;
        existing.Description = doc.Description;
        existing.ImageUrl = doc.ImageUrl;

        _context.SaveChanges();

        return Ok();
    }

}