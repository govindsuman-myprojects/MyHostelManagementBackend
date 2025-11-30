using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Services.Implementations;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Implementations;
using MyHostelManagement.Api.Repositories.Interfaces;

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
var jwtKey = config["Jwt:Key"] ?? throw new Exception("Missing Jwt:Key");
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

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow static files (for mockup / docs)
builder.Services.AddDirectoryBrowser();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//// Fallter flow
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
//});
//app.UseCors("AllowAll");

app.UseStaticFiles(); // serve wwwroot
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

