using ApplicationSecurityAssignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApplicationSecurityAssignment.Services
{
    public class AuditLogService
    {
        private readonly AuthDbContext _dbContext;

        public AuditLogService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogLoginAsync(string userId)
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                Timestamp = DateTime.Now,
                ActionType = "Login"
            };
            _dbContext.AuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LogLogoutAsync(string userId)
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                Timestamp = DateTime.Now,
                ActionType = "Logout"
            };
            _dbContext.AuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}
