//using HospitalMVC.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Text;
//using System.Text.Json;

//namespace HospitalMVC.Controllers
//{
//    public class AdminController : Controller
//    {
//        // 👉 DASHBOARD PAGE
//        public async Task<IActionResult> Dashboard()
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                // 🔥 USERS COUNT
//                var userRes = await client.GetAsync("https://batras-healthcare-system.onrender.com/api/user/all");
//                var userData = await userRes.Content.ReadAsStringAsync();

//                var users = JsonSerializer.Deserialize<List<User>>(userData, new JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                });

//                ViewBag.TotalUsers = users?.Count ?? 0;

//                // 🔥 APPOINTMENTS COUNT
//                var appRes = await client.GetAsync("https://batras-healthcare-system.onrender.com/api/appointment/all");
//                var appData = await appRes.Content.ReadAsStringAsync();

//                var apps = JsonSerializer.Deserialize<List<Appointment>>(appData, new JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                });

//                ViewBag.TotalAppointments = apps?.Count ?? 0;

//                return View();
//            }
//        }

//        // 👉 USERS LIST PAGE
//        public async Task<IActionResult> Users()
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var response = await client.GetAsync("https://batras-healthcare-system.onrender.com/api/user/all");

//                if (!response.IsSuccessStatusCode)
//                    return View(new List<User>());

//                var data = await response.Content.ReadAsStringAsync();

//                var users = JsonSerializer.Deserialize<List<User>>(data, new JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                });

//                return View(users);
//            }
//        }

//        // 👉 ACTIVATE USER
//        public async Task<IActionResult> Activate(int id)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                await client.PutAsync($"https://batras-healthcare-system.onrender.com/api/user/activate/{id}", null);
//            }

//            return RedirectToAction("Users");
//        }

//        // 👉 DEACTIVATE USER
//        public async Task<IActionResult> Deactivate(int id)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                await client.PutAsync($"https://batras-healthcare-system.onrender.com/api/user/deactivate/{id}", null);
//            }

//            return RedirectToAction("Users");
//        }

//        // 👉 DELETE APPOINTMENT
//        public async Task<IActionResult> Delete(int id)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                await client.DeleteAsync($"https://batras-healthcare-system.onrender.com/api/appointment/delete/{id}");
//            }

//            return RedirectToAction("Dashboard");
//        }

//        // 👉 UPDATE APPOINTMENT STATUS
//        public async Task<IActionResult> UpdateStatus(int id, string status)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                await client.PutAsync(
//                    $"https://batras-healthcare-system.onrender.com/api/appointment/status/{id}?status={status}",
//                    null);
//            }

//            return RedirectToAction("Dashboard");
//        }

//        // 👉 LOGOUT
//        public IActionResult Logout()
//        {
//            HttpContext.Session.Clear();
//            return RedirectToAction("Login", "Account");
//        }

//        public async Task<IActionResult> ToggleStatus(int id)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                await client.PutAsync($"https://batras-healthcare-system.onrender.com/api/user/toggle/{id}", null);
//            }

//            return RedirectToAction("Users");
//        }

//        public async Task<IActionResult> Appointments()
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var res = await client.GetAsync("https://batras-healthcare-system.onrender.com/api/appointment/all");
//                var data = await res.Content.ReadAsStringAsync();

//                var list = JsonSerializer.Deserialize<List<Appointment>>(data, new JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                });

//                return View(list);
//            }
//        }



//        public async Task<IActionResult> Doctors()
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var res = await client.GetAsync("https://batras-healthcare-system.onrender.com/api/appointment/alldoctors");
//                var data = await res.Content.ReadAsStringAsync();

//                var list = JsonSerializer.Deserialize<List<Doctor>>(data, new JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                });

//                return View(list);
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddDoctor(Doctor doc)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var json = JsonSerializer.Serialize(doc);
//                var content = new StringContent(json, Encoding.UTF8, "application/json");

//                var res = await client.PostAsync("https://batras-healthcare-system.onrender.com/api/appointment/adddoctor", content);

//                if (res.IsSuccessStatusCode)
//                    TempData["Success"] = "Doctor added successfully!";
//                else
//                    TempData["Error"] = "Failed to add doctor!";
//            }

//            return RedirectToAction("Doctors");
//        }

//        public async Task<IActionResult> DeleteDoctor(int id)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var res = await client.DeleteAsync($"https://batras-healthcare-system.onrender.com/api/appointment/deletedoctor/{id}");

//                if (res.IsSuccessStatusCode)
//                    TempData["Success"] = "Doctor deleted successfully!";
//                else
//                    TempData["Error"] = "Delete failed!";
//            }

//            return RedirectToAction("Doctors");
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditDoctor(Doctor doc)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var json = JsonSerializer.Serialize(doc);
//                var content = new StringContent(json, Encoding.UTF8, "application/json");

//                var res = await client.PutAsync("https://batras-healthcare-system.onrender.com/api/appointment/updatedoctor", content);

//                if (res.IsSuccessStatusCode)
//                    TempData["Success"] = "Doctor updated successfully!";
//                else
//                    TempData["Error"] = "Update failed!";
//            }

//            return RedirectToAction("Doctors");
//        }


//        public async Task<IActionResult> ContactMessages()
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var res = await client.GetAsync("https://batras-healthcare-system.onrender.com/api/contact/all");

//                var data = await res.Content.ReadAsStringAsync();

//                var list = JsonSerializer.Deserialize<List<ContactMessage>>(data,
//                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//                return View(list);
//            }
//        }



//    }
//}



using HospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HospitalMVC.Controllers
{
    public class AdminController : Controller
    {
        // 👉 DASHBOARD PAGE
        //public async Task<IActionResult> Dashboard()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        // USERS COUNT
        //        var userRes =
        //            await client.GetAsync("https://batras-healthcare-system.onrender.com/api/user/all");

        //        var userData =
        //            await userRes.Content.ReadAsStringAsync();

        //        var users =
        //            JsonSerializer.Deserialize<List<User>>(userData,
        //            new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //        ViewBag.TotalUsers = users?.Count ?? 0;

        //        // APPOINTMENTS COUNT
        //        var appRes =
        //            await client.GetAsync("https://batras-healthcare-system.onrender.com/api/appointment/all");

        //        var appData =
        //            await appRes.Content.ReadAsStringAsync();

        //        var apps =
        //            JsonSerializer.Deserialize<List<Appointment>>(appData,
        //            new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //        ViewBag.TotalAppointments = apps?.Count ?? 0;

        //        return View();
        //    }
        //}

        public async Task<IActionResult> Dashboard()
        {
            using (HttpClient client = new HttpClient())
            {
                // USERS
                var usersResponse =
                    await client.GetAsync(
                    "https://batras-healthcare-system.onrender.com/api/appointment/users");

                var usersData =
                    await usersResponse.Content.ReadAsStringAsync();

                var users =
                    JsonSerializer.Deserialize<List<User>>(
                        usersData,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                // APPOINTMENTS
                var appointmentResponse =
                    await client.GetAsync(
                    "https://batras-healthcare-system.onrender.com/api/appointment/all");

                var appointmentData =
                    await appointmentResponse.Content.ReadAsStringAsync();

                var appointments =
                    JsonSerializer.Deserialize<List<Appointment>>(
                        appointmentData,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                // DOCTORS
                var doctorResponse =
                    await client.GetAsync(
                    "https://batras-healthcare-system.onrender.com/api/appointment/alldoctors");

                var doctorData =
                    await doctorResponse.Content.ReadAsStringAsync();

                var doctors =
                    JsonSerializer.Deserialize<List<Doctor>>(
                        doctorData,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                // COUNTS
                ViewBag.TotalUsers = users?.Count ?? 0;

                ViewBag.TotalDoctors = doctors?.Count ?? 0;

                ViewBag.TotalAppointments = appointments?.Count ?? 0;

                ViewBag.PendingAppointments =
                    appointments?.Count(x => x.Status == "Pending") ?? 0;

                ViewBag.ApprovedAppointments =
                    appointments?.Count(x => x.Status == "Approved") ?? 0;

                ViewBag.RejectedAppointments =
                    appointments?.Count(x => x.Status == "Rejected") ?? 0;

                // RECENT APPOINTMENTS
                ViewBag.RecentAppointments =
                    appointments?
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .ToList();

                return View();
            }
        }




        // 👉 USERS LIST PAGE
        public async Task<IActionResult> Users()
        {
            using (HttpClient client = new HttpClient())
            {
                var response =
                    await client.GetAsync("https://batras-healthcare-system.onrender.com/api/user/all");

                if (!response.IsSuccessStatusCode)
                    return View(new List<User>());

                var data =
                    await response.Content.ReadAsStringAsync();

                var users =
                    JsonSerializer.Deserialize<List<User>>(data,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return View(users);
            }
        }

        // 👉 ACTIVATE USER
        public async Task<IActionResult> Activate(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.PutAsync(
                    $"https://batras-healthcare-system.onrender.com/api/user/activate/{id}",
                    null);
            }

            return RedirectToAction("Users");
        }

        // 👉 DEACTIVATE USER
        public async Task<IActionResult> Deactivate(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.PutAsync(
                    $"https://batras-healthcare-system.onrender.com/api/user/deactivate/{id}",
                    null);
            }

            return RedirectToAction("Users");
        }

        // 👉 DELETE APPOINTMENT
        public async Task<IActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.DeleteAsync(
                    $"https://batras-healthcare-system.onrender.com/api/appointment/delete/{id}");
            }

            return RedirectToAction("Dashboard");
        }

        // 👉 UPDATE APPOINTMENT STATUS
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.PutAsync(
                    $"https://batras-healthcare-system.onrender.com/api/appointment/status/{id}?status={status}",
                    null);
            }

            return RedirectToAction("Dashboard");
        }

        // 👉 LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        // 👉 TOGGLE USER STATUS
        public async Task<IActionResult> ToggleStatus(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.PutAsync(
                    $"https://batras-healthcare-system.onrender.com/api/user/toggle/{id}",
                    null);
            }

            return RedirectToAction("Users");
        }

        // 👉 APPOINTMENTS PAGE
        public async Task<IActionResult> Appointments()
        {
            using (HttpClient client = new HttpClient())
            {
                var res =
                    await client.GetAsync("https://batras-healthcare-system.onrender.com/api/appointment/all");

                var data =
                    await res.Content.ReadAsStringAsync();

                var list =
                    JsonSerializer.Deserialize<List<Appointment>>(data,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return View(list);
            }
        }

        // ======================================================
        // 👉 DOCTORS MODULE
        // ======================================================

        // 👉 DOCTORS PAGE
        public async Task<IActionResult> Doctors()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response =
                        await client.GetAsync(
                        "https://batras-healthcare-system.onrender.com/api/appointment/alldoctors");

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

                    return View(doctors ?? new List<Doctor>());
                }
                catch
                {
                    return View(new List<Doctor>());
                }
            }
        }

        // 👉 ADD DOCTOR
        [HttpPost]
        public async Task<IActionResult> AddDoctor(Doctor doc)
        {
            using (HttpClient client = new HttpClient())
            {
                var json =
                    JsonSerializer.Serialize(doc);

                var content =
                    new StringContent(json,
                    Encoding.UTF8,
                    "application/json");

                var res =
                    await client.PostAsync(
                        "https://batras-healthcare-system.onrender.com/api/appointment/adddoctor",
                        content);

                if (res.IsSuccessStatusCode)
                    TempData["Success"] = "Doctor added successfully!";
                else
                    TempData["Error"] = "Failed to add doctor!";
            }

            return RedirectToAction("Doctors");
        }

        // 👉 DELETE DOCTOR
        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res =
                    await client.DeleteAsync(
                        $"https://batras-healthcare-system.onrender.com/api/appointment/deletedoctor/{id}");

                if (res.IsSuccessStatusCode)
                    TempData["Success"] = "Doctor deleted successfully!";
                else
                    TempData["Error"] = "Delete failed!";
            }

            return RedirectToAction("Doctors");
        }

        // 👉 EDIT DOCTOR
        [HttpPost]
        public async Task<IActionResult> EditDoctor(Doctor doc)
        {
            using (HttpClient client = new HttpClient())
            {
                var json =
                    JsonSerializer.Serialize(doc);

                var content =
                    new StringContent(json,
                    Encoding.UTF8,
                    "application/json");

                var res =
                    await client.PutAsync(
                        "https://batras-healthcare-system.onrender.com/api/appointment/updatedoctor",
                        content);

                if (res.IsSuccessStatusCode)
                    TempData["Success"] = "Doctor updated successfully!";
                else
                    TempData["Error"] = "Update failed!";
            }

            return RedirectToAction("Doctors");
        }

        // ======================================================
        // 👉 CONTACT MESSAGES
        // ======================================================

        public async Task<IActionResult> ContactMessages()
        {
            using (HttpClient client = new HttpClient())
            {
                var res =
                    await client.GetAsync(
                        "https://batras-healthcare-system.onrender.com/api/contact/all");

                var data =
                    await res.Content.ReadAsStringAsync();

                var list =
                    JsonSerializer.Deserialize<List<ContactMessage>>(data,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return View(list);
            }
        }
    }
}