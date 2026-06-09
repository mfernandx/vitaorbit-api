using Microsoft.EntityFrameworkCore;
using VitaOrbitApi.Models;

namespace VitaOrbitApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<SymptomRecord> SymptomRecords { get; set; }
        public DbSet<EnvironmentalCondition> EnvironmentalConditions { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Emergency> Emergencies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Gender).HasMaxLength(50);
                entity.Property(u => u.UserDescription).HasMaxLength(500);
                entity.Property(u => u.CurrentLocation).HasMaxLength(150);
                entity.Property(u => u.PhoneNumber).HasMaxLength(30);
                entity.Property(u => u.EmergencyContact).HasMaxLength(30);
                entity.HasMany(u => u.HealthRecords).WithOne(h => h.User).HasForeignKey(h => h.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(u => u.SymptomRecords).WithOne(h => h.User).HasForeignKey(h => h.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HealthRecord>(entity =>
            {
                entity.HasKey(h => h.HealthRecordId);
                entity.Property(h => h.BodyTemperature).HasColumnType("decimal(5,2)");
                entity.Property(h => h.HydrationLevel).HasColumnType("decimal(5,2)");
                entity.Property(h => h.SleepHours).HasColumnType("decimal(5,2)");
                entity.Property(h => h.Notes).HasMaxLength(500);
                entity.Property(h => h.RiskClassification).HasMaxLength(50);
                entity.HasOne(h => h.User).WithMany(u => u.HealthRecords).HasForeignKey(h => h.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SymptomRecord>(entity =>
            {
                entity.HasKey(s => s.SymptomRecordId);
                entity.Property(s => s.SymptomName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Frequency).IsRequired().HasMaxLength(50);
                entity.Property(s => s.Description).HasMaxLength(500);
                entity.Property(s => s.RiskClassification).HasMaxLength(50);
                entity.HasOne(s => s.User).WithMany(u => u.SymptomRecords).HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EnvironmentalCondition>(entity =>
            {
                entity.HasKey(e => e.EnvironmentalConditionId);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.ExternalTemperature).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Humidity).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Altitude).HasColumnType("decimal(8,2)");
                entity.Property(e => e.AtmosphericPressure).HasColumnType("decimal(8,2)");
                entity.Property(e => e.AirQuality).HasMaxLength(100);
                entity.Property(e => e.RadiationLevel).HasColumnType("decimal(8,2)");
                entity.Property(e => e.EnvironmentType).HasMaxLength(100);
                entity.HasOne(e => e.User).WithOne(u => u.EnvironmentalCondition).HasForeignKey<EnvironmentalCondition>(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Emergency>(entity =>
            {
                entity.HasKey(e => e.EmergencyId);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.RequestDate).IsRequired();
                entity.HasOne(e => e.User).WithMany(u => u.Emergencies).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Alert>(entity =>
            {
                entity.HasKey(a => a.AlertId);
                entity.Property(a => a.TypeAlert).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Message).IsRequired().HasMaxLength(500);
                entity.Property(a => a.RiskLevel).IsRequired().HasMaxLength(50);
                entity.Property(a => a.RegisteredAt).IsRequired();
                entity.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(a => a.HealthRecord).WithMany(h => h.Alerts).HasForeignKey(a => a.HealthRecordId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(a => a.SymptomRecord).WithMany(s => s.Alerts).HasForeignKey(a => a.SymptomRecordId).OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
