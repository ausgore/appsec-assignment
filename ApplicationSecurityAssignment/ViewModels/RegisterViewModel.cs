using System.ComponentModel.DataAnnotations;

namespace ApplicationSecurityAssignment.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$",
            ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        [Display(Name = "Credit Card Number")]
        public string CreditCard { get; set; }

        [Required]
        [Display(Name = "Delivery Address")]
        public string Address { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "About Me cannot exceed 500 characters.")]
        [Display(Name = "About Me")]
        public string AboutMe { get; set; }

        [Required]
        [Display(Name = "Photo")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{12,}",
            ErrorMessage = "Password must be at least 12 characters and contain a mix of uppercase, lowercase, special characters, and numbers.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set;  }
    }
}
