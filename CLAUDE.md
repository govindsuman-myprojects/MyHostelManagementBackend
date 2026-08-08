# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

ASP.NET Core 8 Web API backend for a multi-tenant hostel/PG management system. A single `Hostel` (owner) has many `User` records (tenants/staff), `Room`s, `Payment`s, `Complaint`s, `Expense`s, `Announcement`s, and subscribes to a `SubscriptionPlan`. Almost every domain query is scoped by `hostelId`.

## Commands

Run all commands from the repo root or `MyHostelManagement/`.

```
dotnet restore MyHostelManagement
dotnet build MyHostelManagement --configuration Release
dotnet run --project MyHostelManagement          # local run, launch profile picks the port
dotnet publish MyHostelManagement --configuration Release --output <path>
```

There is **no test project** in `MyHostelManagement.slnx` — `dotnet test` is a no-op even though the CI workflow (`.github/workflows/myhostel-management-api.yml`) invokes it. Don't assume test coverage exists; verify behavior by reading the service/repository code or exercising the API via `MyHostelManagement.http` / Swagger.

Swagger UI is only enabled in the Development environment, at the app root when run locally (`app.UseSwaggerUI()` in `Program.cs`).

There are no EF Core Migrations in the repo (no `Migrations/` folder) — the Postgres schema (hosted on Neon) is managed outside of `dotnet ef migrations`. Don't add `dotnet ef migrations add` as a normal workflow step unless the user explicitly starts using migrations; assume the DB schema already matches `ApplicationDbContext.OnModelCreating`.

## Architecture

Standard layered flow: **Controller → Service (interface + implementation) → Repository (interface + implementation) → `ApplicationDbContext`**.

- `Controllers/` — thin ASP.NET controllers, one per resource. Most routes are `[Authorize]` with `[Authorize(Roles = "Owner")]` or `[Authorize(Roles = "Tenant")]` on top; `AuthController` and `OtpController` are `[AllowAnonymous]`.
- `Services/Interfaces` + `Services/Implementations` — business logic, cross-entity orchestration (e.g. `PaymentService` calls into `IUserService`, `IRoomService`, `INotificationService` to compute pending/received payments and rent due dates).
- `Repositories/Interfaces` + `Repositories/Implementations` — direct EF Core access via `ApplicationDbContext`. No generic repository/unit-of-work is registered (see the commented-out line in `Program.cs`) — each entity has its own repository.
- `DTOs/` — request/response shapes; controllers and services should not leak EF entities directly to clients.
- `Profiles/MappingProfile.cs` — AutoMapper profile, registered globally in `Program.cs` via `AddAutoMapper(typeof(Program).Assembly)`. Some services (e.g. `PaymentService`) hand-map instead of using AutoMapper — follow whichever pattern the file you're editing already uses.
- `Data/ApplicationDbContext.cs` — single `IdentityDbContext<ApplicationUser>` with explicit `ToTable`/`HasColumnName` snake_case mappings for every entity (the DB uses `snake_case`, C# uses PascalCase). When adding a new entity/column, register the mapping here explicitly; nothing is convention-based.
- `Models/Common/BaseEntity.cs` — abstract base with `CreatedAt`/`UpdatedAt`; `ApplicationDbContext.SaveChanges`/`SaveChangesAsync` auto-populate these via `ApplyUtcTimestamps()` for any tracked `BaseEntity`. Don't set these fields manually in services.

### Auth model (two identity systems coexist — know which one is live)

- ASP.NET Identity (`AddIdentity<ApplicationUser>`) is wired up in `Program.cs` and backs `ApplicationDbContext`, but the actual login path does **not** use it.
- Real login (`AuthService.LoginAsync`) reads the domain `User` table directly, verifies password via manual `HMACSHA256` against `PasswordHash`/`PasswordSalt` columns, then mints a JWT via `JwtTokenService`. `Hostel` (the owner) also has its own `PasswordHash`/`PasswordSalt` and a separate `JwtTokenService.GenerateToken(Hostel, ...)` overload.
- JWT claims used everywhere downstream: `ClaimTypes.Role` (`"Owner"` or `"Tenant"`), `hostelId`, and `userId` (tenant tokens only). Controllers read these via `User.FindFirst("hostelId")` / `User.FindFirst("userId")` — see `DashboardController` for the canonical example of splitting behavior by role using the same route prefix.
- Login also enforces hostel subscription status: `AuthService.LoginAsync` calls `ISubscriptionService.GetCurrentSubscription(hostelId)` and rejects login with `ApiException(403)` if there's no active/unexpired subscription.
- `ApiException` (`Services/ApiException.cs`) carries an HTTP status code; catch it in controllers to return the right status + `{ success, message }` body (see `AuthController`). Some controllers instead catch generic `Exception` and return `BadRequest(ex.Message)` (see `PaymentController.Create`) — inconsistent, follow the neighboring pattern in the file you're touching rather than "fixing" it project-wide unless asked.

### Known inconsistencies (don't silently "fix" these unless asked — they're pervasive, not local bugs)

- **Namespaces are split** between `MyHostelManagement.X` and `MyHostelManagement.Api.X` for the same kinds of files (e.g. `HostelsController`/`RoomsController`/`PaymentController` use `MyHostelManagement.Api.Controllers`; every other controller uses `MyHostelManagement.Controllers`). Match whatever namespace the surrounding file/folder already uses.
- Two files have a trailing space in their filename before the extension: `Services/Implementations/AuthService .cs` and `Services/Implementations/JwtTokenService .cs`. Reference them by exact filename when using file tools.
- Error handling per controller is inconsistent (`ApiException` + explicit status code vs. bare `catch (Exception)` + `BadRequest`) — see above.

## Configuration

- `appsettings.json` / `appsettings.Development.json` hold `ConnectionStrings:DefaultConnection` (Postgres/Neon via `Npgsql`), `Jwt:Key/Issuer/Audience/AccessTokenExpiryMinutes`, and `Twilio` (OTP) settings. **`appsettings.Development.json` currently contains live-looking secrets (DB password, JWT key) committed to the repo** — never copy these into new files, logs, or messages, and flag it if asked to touch that file.
- `Program.cs` falls back to `Environment.GetEnvironmentVariable("Jwt__Key")` then a hardcoded dev default if `Jwt:Key` is missing from config — production deployments are expected to supply `Jwt__Key` as an env var.
- CORS is fully open (`AllowAnyOrigin/Header/Method`, policy `"AllowAll"`).
- Deployment: `Dockerfile` builds/publishes for containerized hosting; `.github/workflows/myhostel-management-api.yml` deploys to an Azure Web App (`myhostel-management-api`) on push to `master`. The local default branch in this checkout is `prod`, and PRs target `main` — confirm which branch actually triggers the Azure workflow before assuming a push will deploy.
