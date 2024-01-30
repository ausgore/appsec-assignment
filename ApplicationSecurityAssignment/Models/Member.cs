using Microsoft.AspNetCore.Identity;

namespace ApplicationSecurityAssignment.Models
{
    public class Member : IdentityUser
    {
        public required string Name { get; set; }
        public required string Gender { get; set; }
        public required string CreditCardNumber { get; set; }
        public required string Address { get; set; }
        public required string AboutMe { get; set; }
        public required byte[] Photo { get; set; }

        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
        public ICollection<PasswordHistory> PasswordHistories { get; set; } = new List<PasswordHistory>();

    }
}
