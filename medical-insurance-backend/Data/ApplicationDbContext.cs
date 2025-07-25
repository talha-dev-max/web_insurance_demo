using Microsoft.EntityFrameworkCore;
using medical_insurance_backend.Models;

namespace medical_insurance_backend.Data
{
    /// <summary>
    /// Database context for medical insurance application
    /// Handles all database operations and entity configurations
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor that accepts DbContextOptions for dependency injection
        /// </summary>
        /// <param name="options">Database context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet for Company entities
        /// </summary>
        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// DbSet for Policy entities
        /// </summary>
        public DbSet<Policy> Policies { get; set; }

        /// <summary>
        /// Configure entity relationships and constraints
        /// </summary>
        /// <param name="modelBuilder">Model builder for entity configuration</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Company entity configuration
            modelBuilder.Entity<Company>(entity =>
            {
                // Create unique index on CrNumber
                entity.HasIndex(c => c.CrNumber)
                      .IsUnique()
                      .HasDatabaseName("IX_Companies_CrNumber");

                // Configure one-to-many relationship with Policies
                entity.HasMany(c => c.Policies)
                      .WithOne(p => p.Company)
                      .HasForeignKey(p => p.CompanyId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configure string properties with specific collations for Arabic support
                entity.Property(c => c.CompanyNameAr)
                      .HasCharSet("utf8mb4")
                      .UseCollation("utf8mb4_unicode_ci");

                entity.Property(c => c.CompanyNameEn)
                      .HasCharSet("utf8mb4")
                      .UseCollation("utf8mb4_unicode_ci");
            });

            // Policy entity configuration
            modelBuilder.Entity<Policy>(entity =>
            {
                // Create unique index on PolicyNumber
                entity.HasIndex(p => p.PolicyNumber)
                      .IsUnique()
                      .HasDatabaseName("IX_Policies_PolicyNumber");

                // Configure decimal precision for PremiumAmount
                entity.Property(p => p.PremiumAmount)
                      .HasPrecision(18, 2);

                // Configure foreign key relationship
                entity.HasOne(p => p.Company)
                      .WithMany(c => c.Policies)
                      .HasForeignKey(p => p.CompanyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed initial data (optional)
            SeedData(modelBuilder);
        }

        /// <summary>
        /// Seed initial data for testing
        /// </summary>
        /// <param name="modelBuilder">Model builder for seeding data</param>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed sample companies for testing
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    CrNumber = "1234567890",
                    CompanyNameEn = "Saudi Medical Insurance Company",
                    CompanyNameAr = "شركة التأمين الطبي السعودية",
                    PhoneNumber = "0512345678",
                    LicenseStartDate = DateTime.Now,
                    Classification = "أقل من 3 ملايين ريال",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Company
                {
                    Id = 2,
                    CrNumber = "9876543210",
                    CompanyNameEn = "Gulf Healthcare Insurance",
                    CompanyNameAr = "شركة الخليج للتأمين الصحي",
                    PhoneNumber = "0598765432",
                    LicenseStartDate = DateTime.Now.AddDays(-30),
                    Classification = "اكثر من 3 ملايين ريال",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }

        /// <summary>
        /// Override SaveChanges to automatically update timestamps
        /// </summary>
        /// <returns>Number of affected records</returns>
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync to automatically update timestamps
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Number of affected records</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Automatically update CreatedAt and UpdatedAt timestamps
        /// </summary>
        private void UpdateTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is Company || x.Entity is Policy)
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    if (entity.Entity is Company company)
                    {
                        company.CreatedAt = now;
                        company.UpdatedAt = now;
                    }
                    else if (entity.Entity is Policy policy)
                    {
                        policy.CreatedAt = now;
                        policy.UpdatedAt = now;
                    }
                }
                else if (entity.State == EntityState.Modified)
                {
                    if (entity.Entity is Company company)
                    {
                        company.UpdatedAt = now;
                    }
                    else if (entity.Entity is Policy policy)
                    {
                        policy.UpdatedAt = now;
                    }
                }
            }
        }
    }
}