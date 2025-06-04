using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InternationalBankAPI.Data;
using Microsoft.EntityFrameworkCore;
using InternationalBankAPI.Models;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// ? Register services — this is the correct place for AddAuthentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication(); // must come before UseAuthorization()
app.UseCors("AllowAll");
app.UseAuthorization();


app.MapControllers();
SeedEmployee(app);

app.Run();

void SeedEmployee(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!context.Employees.Any())
    {
        CreatePasswordHash("password123", out byte[] passwordHash, out byte[] passwordSalt);

        var employee = new Employee
        {
            Username = "adminemployee",
            PasswordHash = passwordHash,
          
        };

        context.Employees.Add(employee);
        context.SaveChanges();
    }
}

void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
{
    using var hmac = new HMACSHA512();
    passwordSalt = hmac.Key;
    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
}

