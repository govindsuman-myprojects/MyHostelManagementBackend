using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Implementations;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Api.Services.Implementations;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Repositories.Implementations;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Implementations;
using MyHostelManagement.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT
// JWT Key — Use environment variable in Render, fallback for development
var jwtKey = config["Jwt:Key"]
             ?? Environment.GetEnvironmentVariable("Jwt__Key")
             ?? "dev-default-key-change-this"; // fallback to avoid build failure

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});




builder.Services.AddAuthorization();

// Repositories & UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Generic + specific repos not necessary because UnitOfWork exposes them, but you may register GenericRepository if required:
// builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Services
builder.Services.AddScoped<IHostelService, HostelService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ILoginService, LoginService>();

// Repositories
builder.Services.AddScoped<IHostelRepository, HostelRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow static files (for mockup / docs)
builder.Services.AddDirectoryBrowser();

// Fallter flow
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseStaticFiles(); // serve wwwroot
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

