using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationSecurityAssignment.Models
{
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string ActionType { get; set; }
        public Member? User { get; set; }
        public required DateTime Timestamp { get; set; }
    }

}