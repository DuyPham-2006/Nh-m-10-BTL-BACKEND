using Microsoft.EntityFrameworkCore;
using PostManagementApp.Models;

namespace PostManagementApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

// DbSet quản lý Nghĩa trang
        public DbSet<Grave> Graves { get; set; } = null!;
        public DbSet<DeceasedPerson> DeceasedPersons { get; set; } = null!;
        public DbSet<Relative> Relatives { get; set; } = null!;

// DbSet quản lý Nước
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<WaterMeter> WaterMeters { get; set; } = null!;
        public DbSet<WaterPriceTier> WaterPriceTiers { get; set; } = null!;
        public DbSet<WaterReading> WaterReadings { get; set; } = null!;
        public DbSet<WaterBill> WaterBills { get; set; } = null!;
        public DbSet<WaterSupply> WaterSupplies { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<UserAccount> UserAccounts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

// ==================== CẤU HÌNH MỘ ====================
            modelBuilder.Entity<Grave>(entity =>
            {
                entity.HasKey(g => g.GraveId);
                entity.Property(g => g.Area).IsRequired().HasMaxLength(50);
                entity.Property(g => g.Status).HasMaxLength(50);
            });

// ==================== CẤU HÌNH NGƯỜI MẤT ====================
            modelBuilder.Entity<DeceasedPerson>(entity =>
            {
                entity.HasKey(d => d.DeceasedId);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Description).HasMaxLength(500);
                
// Một người mất có Một mộ (quan hệ 1-1)
                entity.HasOne(d => d.Grave)
                    .WithOne(g => g.DeceasedPerson)
                    .HasForeignKey<DeceasedPerson>(d => d.GraveId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

// ==================== CẤU HÌNH THÂN NHÂN ====================
            modelBuilder.Entity<Relative>(entity =>
            {
                entity.HasKey(r => r.RelativeId);
                entity.Property(r => r.FullName).IsRequired().HasMaxLength(100);
                entity.Property(r => r.PhoneNumber).HasMaxLength(20);
                entity.Property(r => r.Relationship).HasMaxLength(50);
                entity.Property(r => r.Email).HasMaxLength(100);
                entity.Property(r => r.Address).HasMaxLength(300);
                
// Nhiều thân nhân thuộc về Một người mất (quan hệ 1-N)
                entity.HasOne(r => r.DeceasedPerson)
                    .WithMany(d => d.Relatives)
                    .HasForeignKey(r => r.DeceasedId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

// ==================== DỮ LIỆU MẪU ====================
            modelBuilder.Entity<Grave>().HasData(
                new Grave { GraveId = 1, Area = "A", RowNumber = 1, GraveNumber = 1, Location = "Khu A - Hàng 1 - Mộ 1", Status = "Occupied" },
                new Grave { GraveId = 2, Area = "A", RowNumber = 1, GraveNumber = 2, Location = "Khu A - Hàng 1 - Mộ 2", Status = "Occupied" },
                new Grave { GraveId = 3, Area = "A", RowNumber = 2, GraveNumber = 1, Location = "Khu A - Hàng 2 - Mộ 1", Status = "Occupied" },
                new Grave { GraveId = 4, Area = "B", RowNumber = 1, GraveNumber = 1, Location = "Khu B - Hàng 1 - Mộ 1", Status = "Occupied" },
                new Grave { GraveId = 5, Area = "B", RowNumber = 1, GraveNumber = 2, Location = "Khu B - Hàng 1 - Mộ 2", Status = "Occupied" },
                new Grave { GraveId = 6, Area = "B", RowNumber = 2, GraveNumber = 1, Location = "Khu B - Hàng 2 - Mộ 1", Status = "Occupied" }
            );

            modelBuilder.Entity<DeceasedPerson>().HasData(
                new DeceasedPerson { DeceasedId = 1, Name = "Nguyễn Văn A", BirthYear = 1945, DeathYear = 2020, GraveId = 1, Description = "Công nhân" },
                new DeceasedPerson { DeceasedId = 2, Name = "Trần Thị B", BirthYear = 1950, DeathYear = 2018, GraveId = 2, Description = "Giáo viên" },
                new DeceasedPerson { DeceasedId = 3, Name = "Lê Văn C", BirthYear = 2000, DeathYear = 2020, GraveId = 3, Description = "Sinh viên" },
                new DeceasedPerson { DeceasedId = 4, Name = "Phạm Thị D", BirthYear = 1965, DeathYear = 2023, GraveId = 4, Description = "Nhân viên văn phòng" },
                new DeceasedPerson { DeceasedId = 5, Name = "Hoàng Văn E", BirthYear = 1920, DeathYear = 1995, GraveId = 5, Description = "Nông dân" },
                new DeceasedPerson { DeceasedId = 6, Name = "Tên mới", BirthYear = 1970, DeathYear = 2024, GraveId = 6, Description = "Kỹ sư" }
            );

            modelBuilder.Entity<Relative>().HasData(
                new Relative { RelativeId = 1, DeceasedId = 1, FullName = "Nguyễn Văn B", Relationship = "Con trai", PhoneNumber = "0123456789", Email = "nguyenvB@email.com", Address = "Hà Nội" },
                new Relative { RelativeId = 2, DeceasedId = 1, FullName = "Nguyễn Thị C", Relationship = "Con gái", PhoneNumber = "0987654321", Email = "nguyenthC@email.com", Address = "Hồ Chí Minh" },
                new Relative { RelativeId = 3, DeceasedId = 2, FullName = "Trần Văn D", Relationship = "Chồng", PhoneNumber = "0234567890", Email = "tranD@email.com", Address = "Hà Nội" },
                new Relative { RelativeId = 4, DeceasedId = 3, FullName = "Lê Thị E", Relationship = "Mẹ", PhoneNumber = "0345678901", Email = "leE@email.com", Address = "Đà Nẵng" }
            );

// ==================== CẤU HÌNH QUẢN LÝ NƯỚC ====================
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);
                entity.Property(c => c.FullName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Address).IsRequired().HasMaxLength(255);
                entity.Property(c => c.PhoneNumber).HasMaxLength(15);
                entity.Property(c => c.Email).HasMaxLength(100);
                entity.Property(c => c.MeterCode).HasMaxLength(50);
                entity.Property(c => c.Status).IsRequired().HasMaxLength(20);

                entity.HasIndex(c => c.MeterCode).IsUnique();
            });

            modelBuilder.Entity<WaterMeter>(entity =>
            {
                entity.HasKey(w => w.MeterId);
                entity.Property(w => w.MeterCode).IsRequired().HasMaxLength(50);
                entity.Property(w => w.Brand).HasMaxLength(50);
                entity.Property(w => w.Model).HasMaxLength(50);
                entity.Property(w => w.Status).IsRequired().HasMaxLength(20);

                entity.HasIndex(w => w.MeterCode).IsUnique();

                entity.HasOne(w => w.Customer)
                    .WithMany(c => c.WaterMeters)
                    .HasForeignKey(w => w.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WaterPriceTier>(entity =>
            {
                entity.HasKey(p => p.TierId);
                entity.Property(p => p.TierLevel).IsRequired();
                entity.Property(p => p.FromM3).IsRequired();
                entity.Property(p => p.ToM3).IsRequired();
                entity.Property(p => p.PricePerM3).IsRequired();
                entity.Property(p => p.EffectiveDate).IsRequired();
                entity.Property(p => p.Status).IsRequired().HasMaxLength(20);

                entity.HasIndex(p => new { p.TierLevel, p.EffectiveDate }).IsUnique();
            });

            modelBuilder.Entity<WaterReading>(entity =>
            {
                entity.HasKey(r => r.ReadingId);
                entity.Property(r => r.ReadingMonth).IsRequired();
                entity.Property(r => r.OldIndex).IsRequired();
                entity.Property(r => r.NewIndex).IsRequired();
                entity.Property(r => r.Status).IsRequired().HasMaxLength(20);
                entity.Property(r => r.Notes).HasMaxLength(255);

                entity.HasOne(r => r.WaterMeter)
                    .WithMany(w => w.WaterReadings)
                    .HasForeignKey(r => r.MeterId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.ReadingStaff)
                    .WithMany(u => u.WaterReadings)
                    .HasForeignKey(r => r.ReadingStaffId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(r => new { r.MeterId, r.ReadingMonth }).IsUnique();
            });

            modelBuilder.Entity<WaterBill>(entity =>
            {
                entity.HasKey(b => b.BillId);
                entity.Property(b => b.BillStatus).IsRequired().HasMaxLength(20);

                entity.HasOne(b => b.WaterReading)
                    .WithOne(r => r.WaterBill)
                    .HasForeignKey<WaterBill>(b => b.ReadingId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Customer)
                    .WithMany(c => c.WaterBills)
                    .HasForeignKey(b => b.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.WaterMeter)
                    .WithMany()
                    .HasForeignKey(b => b.MeterId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.ProcessedByUser)
                    .WithMany(u => u.ProcessedBills)
                    .HasForeignKey(b => b.ProcessedBy)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(b => b.BillingMonth);
                entity.HasIndex(b => b.BillStatus);
            });

            modelBuilder.Entity<WaterSupply>(entity =>
            {
                entity.HasKey(s => s.SupplyId);
                entity.Property(s => s.Notes).HasMaxLength(255);

                entity.HasOne(s => s.RecordedByUser)
                    .WithMany(u => u.RecordedSupplies)
                    .HasForeignKey(s => s.RecordedBy)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(s => s.SupplyMonth).IsUnique();
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);
                entity.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(20);
                entity.Property(p => p.Notes).HasMaxLength(255);

                entity.HasOne(p => p.WaterBill)
                    .WithMany()
                    .HasForeignKey(p => p.BillId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Customer)
                    .WithMany(c => c.Payments)
                    .HasForeignKey(p => p.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.ConfirmedByUser)
                    .WithMany(u => u.ConfirmedPayments)
                    .HasForeignKey(p => p.ConfirmedBy)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(p => p.BillId);
                entity.HasIndex(p => p.CustomerId);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).HasMaxLength(100);
                entity.Property(u => u.PhoneNumber).HasMaxLength(15);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Status).IsRequired().HasMaxLength(20);

                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
