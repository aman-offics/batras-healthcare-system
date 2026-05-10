using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔥 ADD SERVICES
builder.Services.AddControllers();

// 🔥 DB CONNECTION
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔥 SWAGGER (optional but useful)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// 🔥 AUTO CREATE ADMIN (SEED DATA)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Users.Any(u => u.Email == "admin@hospital.com"))
    {
        context.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@hospital.com",
            Password = "Admin@123",
            IsActive = true,
            Role = "Admin"
        });

        context.SaveChanges();
    }
}


// 🔥 MIDDLEWARE PIPELINE

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();