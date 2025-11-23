using MyHostelManagement.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MyHostelManagement.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Hostel> Hostels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MessRecord> MessRecords { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // JSON mapping
            builder.Entity<Tenant>().Property(t => t.Extra).HasColumnType("jsonb");
            builder.Entity<AuditLog>().Property(a => a.Details).HasColumnType("jsonb");

            // Indexes
            builder.Entity<Hostel>(entity =>
            {
                entity.ToTable("hostels");   // table name EXACTLY as in PostgreSQL

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id"); // column name

                entity.Property(e => e.Name)
                      .HasColumnName("name");

                entity.Property(e => e.Address)
                      .HasColumnName("address");

                entity.Property(e => e.OwnerName)
                      .HasColumnName("owner_name");

                entity.Property(e => e.Phone)
                      .HasColumnName("phone");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnName("updated_at");
            });
            builder.Entity<Bed>().HasIndex(b => b.Status);
            builder.Entity<Tenant>().HasIndex(t => t.Phone);
            builder.Entity<Payment>().HasIndex(p => new { p.Month, p.Year });

            // Cascade rules
            builder.Entity<Room>().HasOne(r => r.Hostel).WithMany(h => h.Rooms).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Bed>().HasOne(b => b.Room).WithMany(r => r.Beds).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
