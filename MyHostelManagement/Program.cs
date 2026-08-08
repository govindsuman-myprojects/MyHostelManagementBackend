using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyHostelManagement.Data;
using MyHostelManagement.Models;
using MyHostelManagement.Services.Implementations;
using MyHostelManagement.Services.Interfaces;
using MyHostelManagement.Repositories.Implementations;
using MyHostelManagement.Repositories.Interfaces;
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
var jwtKey = config["Jwt:Key"] ?? Environment.GetEnvironmentVariable("Jwt__Key");
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "Jwt:Key is not configured. Set it via appsettings, `dotnet user-secrets set \"Jwt:Key\" \"...\"` for local dev, or the Jwt__Key environment variable in production.");
}

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

// Generic + specific repos not necessary because UnitOfWork exposes them, but you may register GenericRepository if required:
// builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Services
builder.Services.AddScoped<IHostelService, HostelService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IComplaintService, ComplaintService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<ITermsAndConditionsService, TermsAndConditionsService>();
builder.Services.AddScoped<IComplaintCategoryService, ComplaintCategoryService>();
builder.Services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
builder.Services.AddScoped<IAnnouncementTypeService, AnnouncementTypeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

// Repositories
builder.Services.AddScoped<IHostelRepository, HostelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<ITermsAndConditionsRepository, TermsAndConditionsRepository>();
builder.Services.AddScoped<IComplaintCategoryRepository, ComplaintCategoryRepository>();
builder.Services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
builder.Services.AddScoped<IAnnouncementTypeRepository, AnnouncementTypeRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyHostel API",
        Version = "v1"
    });

    // 🔐 JWT AUTH CONFIG
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Allow static files (for mockup / docs)
builder.Services.AddDirectoryBrowser();

// CORS — allowed origins are read from config (Cors:AllowedOrigins) so each
// environment (local dev, production) can list its own frontend URL(s) without a code change.
var allowedOrigins = config.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend",
        b => b.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<MyHostelManagement.Middleware.ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseStaticFiles(); // serve wwwroot
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

