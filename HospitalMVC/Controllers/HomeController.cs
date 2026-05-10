using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using HospitalMVC.Models;
using System.Text.Json;
using System.Text;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View("index");
    }

    public async Task<IActionResult> Appointment()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        using (HttpClient client = new HttpClient())
        {
            // 🔥 GET DOCTORS FROM API
            var res = await client.GetAsync("http://localhost:5113/api/doctor/all");
            var data = await res.Content.ReadAsStringAsync();

            var doctors = JsonSerializer.Deserialize<List<Doctor>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ViewBag.Doctors = doctors; // ✔ pass doctors

            return View(new Appointment()); // 🔥 IMPORTANT FIX
        }
    }
    public IActionResult About()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("about");
    }

    //public IActionResult Appointment()
    //{
    //    var user = HttpContext.Session.GetString("username");

    //    if (user == null)
    //    {
    //        return RedirectToAction("Login", "Account");
    //    }
    //    return View("appointment");
    //}

    public IActionResult Contact()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View("contact");
    }

    public IActionResult Department()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("department");
    }

    public IActionResult Ddepartmentdetails()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("departmentdetails");
    }

    //public IActionResult Doctors()
    //{
    //    if (HttpContext.Session.GetString("username") == null)
    //    {
    //        return RedirectToAction("Login", "Account");
    //    }
    //    return View("doctors");
    //}



    public async Task<IActionResult> Doctors()
    {
        using (HttpClient client = new HttpClient())
        {
            var response =
                await client.GetAsync(
                "http://localhost:5113/api/appointment/alldoctors");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Doctor>());
            }

            var data =
                await response.Content.ReadAsStringAsync();

            var doctors =
                JsonSerializer.Deserialize<List<Doctor>>(
                    data,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(doctors);
        }
    }

    public IActionResult faq()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("faq");
    }

    public IActionResult Gallery()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("gallery");
    }

    public IActionResult Privacy()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View();
    }

    public IActionResult ServiceDetails()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("service-details");
    }

    public IActionResult Services()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("services");
    }

    public IActionResult Starterpage()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("starter-page");
    }

    public IActionResult Terms()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("terms");
    }

    public IActionResult Testimonials()
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View("testimonials");
    }

    [HttpPost]
    public async Task<IActionResult> Contact(ContactMessage msg)
    {
        using (HttpClient client = new HttpClient())
        {
            var json = JsonSerializer.Serialize(msg);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("http://localhost:5113/api/contact/send", content);

            if (res.IsSuccessStatusCode)
                TempData["Success"] = "Message sent successfully!";
            else
                TempData["Error"] = "Something went wrong!";
        }

        return RedirectToAction("Contact");
    }

    public IActionResult Cardiology()
    {
        return View();
    }

    public IActionResult Neurology()
    {
        return View();
    }

    public IActionResult Orthopedics()
    {
        return View();
    }

    public IActionResult Pediatrics()
    {
        return View();
    }

    public IActionResult Emergency()
    {
        return View();
    }

    public IActionResult Oncology()
    {
        return View();
    }
}

