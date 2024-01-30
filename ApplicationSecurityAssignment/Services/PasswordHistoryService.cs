using ApplicationSecurityAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationSecurityAssignment.Services
{
    public class PasswordHistoryService
    {
        private readonly AuthDbContext _dbContext;
        public PasswordHistoryService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPasswordHistory(string userId, string password)
        {
            var passwordHistory = new PasswordHistory
            {
                UserId = userId,
                Timestamp = DateTime.Now,
                Password = password
            };
            _dbContext.PasswordHistories.Add(passwordHistory);
            await _dbContext.SaveChangesAsync();
        }

        public List<PasswordHistory> GetRecentPasswordHistories(string userId)
        {
            var recentPasswordHistory = _dbContext.PasswordHistories
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Timestamp)
                .Take(2)
                .ToList();
            return recentPasswordHistory;
        }
    }
}
