using ApplicationSecurityAssignment.Models;
using ApplicationSecurityAssignment.Services;
using ApplicationSecurityAssignment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationSecurityAssignment.Pages
{
    public class ResetModel : PageModel
    {
        [BindProperty]
        public ResetEmailViewModel ResetEmailViewModel { get; set; }
        private readonly EmailSenderService emailSenderService;
        private UserManager<Member> userManager { get; }

        public ResetModel(EmailSenderService _emailSenderService, UserManager<Member> _userManager)
        {
            emailSenderService = _emailSenderService;
            userManager = _userManager;
        }
        public async void OnGetAsync()
        {
            var receiver = "glenn.emte@gmail.com";
            var subject = "Test";
            var message = "Hello World";
            await emailSenderService.SendEmailAsync(receiver, subject, message);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(ResetEmailViewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError("ResetEmailViewModel.Email", "Email provided is invalid.");
                    return Page();
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Page("/ResetPassword", null, new { email = user.Email, token }, Request.Scheme);
                await emailSenderService.SendEmailAsync(user.Email!, "Reset Password", resetLink);

                ModelState.AddModelError("ResetEmailViewModel.Email", "Reset email has been sent! Please check your email to reset your password.");
            }
            return Page();
        }
    }
}
