using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;

using HospitalMVC.Models;

public class AppointmentController : Controller
{

    [HttpPost]
    public async Task<IActionResult> Book(Appointment app)
    {
        var email = HttpContext.Session.GetString("email");

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("Login", "Account");

        app.Email = email; // 🔥 FIX

        using (HttpClient client = new HttpClient())
        {
            var json = JsonSerializer.Serialize(app);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://batras-healthcare-system.onrender.com/api/appointment/book", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Appointment booked successfully!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync(); // 🔥 API message
                TempData["Error"] = error;
            }
        }

        return RedirectToAction("Appointment", "Home");
    }

    public async Task<IActionResult> Appointments()
    {
        using (HttpClient client = new HttpClient())
        {
            var res = await client.GetAsync("https://batras-healthcare-system.onrender.com/api/appointment/all");

            var data = await res.Content.ReadAsStringAsync();

            var list = JsonSerializer.Deserialize<List<Appointment>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(list);
        }
    }

    public async Task<IActionResult> MyAppointments()
    {
        var email = HttpContext.Session.GetString("email");

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Account");
        }

        using (HttpClient client = new HttpClient())
        {
            var res = await client.GetAsync($"https://batras-healthcare-system.onrender.com/api/appointment/byemail/{email}");

            var data = await res.Content.ReadAsStringAsync();

            var list = JsonSerializer.Deserialize<List<Appointment>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            HttpContext.Session.SetInt32("appointmentCount", list.Count);
            // 🔥 COUNT STORE
            ViewBag.Count = list.Count;

            return View(list);
        }
    }



}