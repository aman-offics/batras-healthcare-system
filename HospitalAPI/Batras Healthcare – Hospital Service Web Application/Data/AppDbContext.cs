using Microsoft.EntityFrameworkCore;
using Batras_Healthcare___Hospital_Service_Web_Application.Models;
using HospitalAPI.Models;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
}