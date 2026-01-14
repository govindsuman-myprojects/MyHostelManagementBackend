using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Models;
using MyHostelManagement.Models.Common;
using System.Reflection.Emit;

namespace MyHostelManagement.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Hostel> Hostels { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ComplaintCategory> ComplaintCategories { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<AnnouncementType> AnnouncementTypes { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<BaseEntity>();
            base.OnModelCreating(builder);

            // JSON mapping
            builder.Entity<AuditLog>().Property(a => a.Details).HasColumnType("jsonb");

            builder.Entity<Hostel>(entity =>
            {
                entity.ToTable("hostels");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.OwnerName).HasColumnName("owner_name");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasMany(e => e.Users)
                      .WithOne(u => u.Hostel)
                      .HasForeignKey(u => u.HostelId);

                entity.HasMany(e => e.Rooms)
                      .WithOne(r => r.Hostel)
                      .HasForeignKey(r => r.HostelId);

                entity.HasMany(e => e.Payments)
                      .WithOne(p => p.Hostel)
                      .HasForeignKey(p => p.HostelId);

                entity.HasMany(e => e.Complaints)
                      .WithOne(c => c.Hostel)
                      .HasForeignKey(c => c.HostelId);

                entity.HasMany(e => e.Expenses)
                      .WithOne(ex => ex.Hostel)
                      .HasForeignKey(ex => ex.HostelId);

                entity.HasMany(e => e.TermsAndConditions)
                      .WithOne(tc => tc.Hostel)
                      .HasForeignKey(tc => tc.HostelId);
            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RoleName).HasColumnName("role_name");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasMany(e => e.Users)
                      .WithOne(u => u.Role)
                      .HasForeignKey(u => u.RoleId);

                entity.HasMany(e => e.TermsAndConditions)
                      .WithOne(tc => tc.Role)
                      .HasForeignKey(tc => tc.RoleId);
            });

            builder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.HostelId).HasColumnName("hostel_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.RentAmount).HasColumnName("rent_amount");
                entity.Property(e => e.RentCycle).HasColumnName("rent_cycle");
                entity.Property(e => e.AdvanceAmount).HasColumnName("advance_amount");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.GurdianName).HasColumnName("gurdian_name");
                entity.Property(e => e.GurdianPhoneNumber).HasColumnName("gurdian_phone_number");
                entity.Property(e => e.AadharCardNumber).HasColumnName("aadhar_card_number");
                entity.Property(e => e.JoinDate).HasColumnName("join_date");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.AadharCardFile).HasColumnName("aadhar_card_file");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
                entity.Property(e => e.RoomId).HasColumnName("room_id");

                // Relationships
                entity.HasOne(e => e.Hostel)
                      .WithMany(h => h.Users)
                      .HasForeignKey(e => e.HostelId);

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RoleId);

                entity.HasMany(e => e.Payments)
                      .WithOne(p => p.User)
                      .HasForeignKey(p => p.UserId);

                entity.HasMany(e => e.Complaints)
                      .WithOne(c => c.User)
                      .HasForeignKey(c => c.UserId);

            });

            builder.Entity<Room>(entity =>
            {
                entity.ToTable("rooms");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.HostelId).HasColumnName("hostel_id");
                entity.Property(e => e.RoomNumber).HasColumnName("room_number");
                entity.Property(e => e.TotalBeds).HasColumnName("total_beds");
                entity.Property(e => e.OccupiedBeds).HasColumnName("occupied_beds");
                entity.Property(e => e.Rent).HasColumnName("rent");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasOne(e => e.Hostel)
                      .WithMany(h => h.Rooms)
                      .HasForeignKey(e => e.HostelId);

                entity.HasMany(e => e.Complaints)
                      .WithOne(c => c.Room)
                      .HasForeignKey(c => c.RoomId);
            });

            builder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.HostelId).HasColumnName("hostel_id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.PaymentMonth).HasColumnName("payment_month");
                entity.Property(e => e.PaymentYear).HasColumnName("payment_year");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Payments)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Hostel)
                      .WithMany(h => h.Payments)
                      .HasForeignKey(e => e.HostelId);
            });

            builder.Entity<ComplaintCategory>(entity =>
            {
                entity.ToTable("complaint_categories");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoryName).HasColumnName("category_name");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasMany(e => e.Complaints)
                      .WithOne(c => c.Category)
                      .HasForeignKey(c => c.CategoryId);
            });

            builder.Entity<Complaint>(entity =>
            {
                entity.ToTable("complaints");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.HostelId).HasColumnName("hostel_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.RoomId).HasColumnName("room_id");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.Content).HasColumnName("content");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasOne(e => e.Hostel)
                      .WithMany(h => h.Complaints)
                      .HasForeignKey(e => e.HostelId);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Complaints)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Room)
                      .WithMany(r => r.Complaints)
                      .HasForeignKey(e => e.RoomId);

                entity.HasOne(e => e.Category)
                      .WithMany(cc => cc.Complaints)
                      .HasForeignKey(e => e.CategoryId);
            });

            builder.Entity<ExpenseCategory>(entity =>
            {
                entity.ToTable("expense_categories");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoryName).HasColumnName("category_name");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasMany(e => e.Expenses)
                      .WithOne(ex => ex.ExpenseCategory)
                      .HasForeignKey(ex => ex.ExpenseCategoryId);
            });

            builder.Entity<Expense>(entity =>
            {
                entity.ToTable("expenses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.HostelId).HasColumnName("hostel_id");
                entity.Property(e => e.ExpenseCategoryId).HasColumnName("expense_category");
                entity.Property(e => e.ExpenseSubCategory).HasColumnName("expense_sub_category");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasOne(e => e.Hostel)
                      .WithMany(h => h.Expenses)
                      .HasForeignKey(e => e.HostelId);

                entity.HasOne(e => e.ExpenseCategory)
                      .WithMany(ec => ec.Expenses)
                      .HasForeignKey(e => e.ExpenseCategoryId);
            });

            builder.Entity<AnnouncementType>(entity =>
            {
                entity.ToTable("announcements_type");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TypeName).HasColumnName("type_name");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasMany(e => e.Announcements)
                      .WithOne(a => a.Type)
                      .HasForeignKey(a => a.TypeId);
            });

            builder.Entity<Announcement>(entity =>
            {
                entity.ToTable("announcements");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TypeId).HasColumnName("type_id");
                entity.Property(e => e.Subject).HasColumnName("subject");
                entity.Property(e => e.Message).HasColumnName("message");
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasOne(e => e.Type)
                      .WithMany(t => t.Announcements)
                      .HasForeignKey(e => e.TypeId);
            });

            builder.Entity<TermsAndConditions>(entity =>
            {
                entity.ToTable("terms_and_conditions");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.HostelId).HasColumnName("hostel_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.Content).HasColumnName("content");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relationships
                entity.HasOne(e => e.Hostel)
                      .WithMany(h => h.TermsAndConditions)
                      .HasForeignKey(e => e.HostelId);

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.TermsAndConditions)
                      .HasForeignKey(e => e.RoleId);
            });
        }

        public override int SaveChanges()
        {
            ApplyUtcTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            ApplyUtcTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyUtcTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
