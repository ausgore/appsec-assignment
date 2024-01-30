using System.ComponentModel.DataAnnotations;

namespace ApplicationSecurityAssignment.ViewModels
{
    public class ResetEmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }
}
