using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationSecurityAssignment.Models
{
    public class PasswordHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string UserId { get; set; }
        public Member? User { get; set; }
        public required DateTime Timestamp { get; set; }

        public required string Password { get; set; }
    }
}
