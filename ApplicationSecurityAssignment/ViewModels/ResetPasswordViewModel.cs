using System.ComponentModel.DataAnnotations;

namespace ApplicationSecurityAssignment.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{12,}",
            ErrorMessage = "Password must be at least 12 characters and contain a mix of uppercase, lowercase, special characters, and numbers.")]
        public string Password { get; set; }
    }
}
