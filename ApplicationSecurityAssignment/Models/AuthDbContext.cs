using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApplicationSecurityAssignment.Models
{
    public class AuthDbContext : IdentityDbContext<Member>
    {
        private readonly IConfiguration _configuration;
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<PasswordHistory> PasswordHistories { get; set; }

        public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AuditLog>().HasKey(a => a.Id);
            builder.Entity<AuditLog>().HasOne(a => a.User).WithMany(u => u.AuditLogs).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<PasswordHistory>().HasKey(p => p.Id);
            builder.Entity<PasswordHistory>().HasOne(p => p.User).WithMany(u => u.PasswordHistories).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("AuthConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
