using HospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace HospitalMVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 🔥 LOGIN POST
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                var encodedEmail = Uri.EscapeDataString(email);
                var encodedPassword = Uri.EscapeDataString(password);

                var url =
                    $"https://batras-healthcare-system.onrender.com/api/user/login?email={encodedEmail}&password={encodedPassword}";

                var res = await client.GetAsync(url);

                if (!res.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Invalid login!";
                    return View();
                }

                var data = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(data))
                {
                    ViewBag.Error = "Invalid login!";
                    return View();
                }

                var user =
                    JsonSerializer.Deserialize<User>(
                        data,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (user != null)
                {
                    // SESSION
                    HttpContext.Session.SetString(
                        "email",
                        user.Email);

                    HttpContext.Session.SetString(
                        "username",
                        user.Name);

                    // APPOINTMENT COUNT FETCH
                    var appointmentResponse =
                        await client.GetAsync(
                        $"https://batras-healthcare-system.onrender.com/api/appointment/byemail/{user.Email}");

                    if (appointmentResponse.IsSuccessStatusCode)
                    {
                        var appointmentData =
                            await appointmentResponse.Content.ReadAsStringAsync();

                        var appointments =
                            JsonSerializer.Deserialize<List<Appointment>>(
                                appointmentData,
                                new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                        HttpContext.Session.SetInt32(
                            "AppointmentCount",
                            appointments.Count);
                    }

                    // ADMIN
                    if (user.Role == "Admin")
                        return RedirectToAction(
                            "Dashboard",
                            "Admin");

                    // USER
                    return RedirectToAction(
                        "Index",
                        "Home");
                }

                ViewBag.Error = "Invalid login!";
                return View();
            }
        }
        // 🔥 REGISTER PAGE
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 🔥 REGISTER POST
        //[HttpPost]
        //public async Task<IActionResult> Register(User user)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var json = JsonSerializer.Serialize(user);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await client.PostAsync("https://batras-healthcare-system.onrender.com/api/user/register", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["Success"] = "Registered Successfully ✅";
        //            return RedirectToAction("Login");
        //        }
        //    }

        //    ViewBag.Error = "Registration failed ❌";
        //    return View();
        //}



        [HttpPost]
        public async Task<IActionResult> Register(User user, string confirmPassword)
        {
            // CHECK PASSWORD MATCH
            if (user.Password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match!";
                return View();
            }

            // DEFAULT ROLE
            user.Role = "User";

            using (HttpClient client = new HttpClient())
            {
                // API CALL
                var response =
                    await client.PostAsJsonAsync(
                    "https://batras-healthcare-system.onrender.com/api/user/register",
                    user);

                // SUCCESS
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] =
                        "Registration successful!";

                    return RedirectToAction("Login");
                }

                // FAILED
                ViewBag.Error = "Registration failed ❌";

                return View();
            }
        }
        // 🔥 LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }



        [HttpPost]
        public async Task<IActionResult> AddDoctor(Doctor doc)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonSerializer.Serialize(doc);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync("https://batras-healthcare-system.onrender.com/api/appointment/adddoctor", content);

                if (!res.IsSuccessStatusCode)
                {
                    Console.WriteLine("ADD FAILED");
                }
                else
                {
                    Console.WriteLine("ADD SUCCESS");
                }
            }

            return RedirectToAction("Doctors");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.DeleteAsync($"https://batras-healthcare-system.onrender.com/api/appointment/deletedoctor/{id}");
            }

            return RedirectToAction("Doctors");
        }

        public async Task<IActionResult> EditDoctor(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync("https://batras-healthcare-system.onrender.com/api/appointment/alldoctors");
                var data = await res.Content.ReadAsStringAsync();

                var list = JsonSerializer.Deserialize<List<Doctor>>(data, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var doc = list.FirstOrDefault(x => x.Id == id);

                return View(doc);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditDoctor(Doctor doc)
        {
            Console.WriteLine("ID: " + doc.Id);
            Console.WriteLine("Name: " + doc.Name);
            Console.WriteLine("Spec: " + doc.Specialization);

            using (HttpClient client = new HttpClient())
            {
                var json = JsonSerializer.Serialize(doc);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PutAsync("https://batras-healthcare-system.onrender.com/api/appointment/updatedoctor", content);

                if (!res.IsSuccessStatusCode)
                    TempData["Error"] = "Update failed!";
                else
                    TempData["Success"] = "Updated successfully!";
            }

            return RedirectToAction("Doctors");
        }
    }
}