using ApplicationSecurityAssignment.Models;
using ApplicationSecurityAssignment.Services;
using ApplicationSecurityAssignment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationSecurityAssignment.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public ResetPasswordViewModel ResetPasswordViewModel { get; set; }
        public UserManager<Member> userManager { get; set; }
        public PasswordHistoryService passwordHistoryService { get; }

        public ResetPasswordModel(UserManager<Member> _userManager, PasswordHistoryService _passwordHistoryService)
        {
            userManager = _userManager;
            passwordHistoryService = _passwordHistoryService;
        }

        public async Task<IActionResult> OnGetAsync(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token)) return Page();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return RedirectToPage("Login");

            var isTokenValid = await userManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
            if (!isTokenValid) return RedirectToPage("Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var email = Request.Query["email"];
                var token = Request.Query["token"];
                var user = await userManager.FindByEmailAsync(email!);

                var passwordHistory = passwordHistoryService.GetRecentPasswordHistories(user!.Id);
                if ((DateTime.Now - passwordHistory.First().Timestamp).TotalMinutes < 2)
                {
                    ModelState.AddModelError("ResetPasswordViewModel.Password", "You're changing your password too quickly. Please wait for at least 2 minutes before changing it again.");
                    return Page();
                }

                bool isPasswordMatched = false;
                foreach (var history in passwordHistory)
                {
                    var hash = new PasswordHasher<Member>();
                    var verificationResult = hash.VerifyHashedPassword(user, history.Password, ResetPasswordViewModel.Password);
                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        isPasswordMatched = true;
                        break;
                    }
                }

                if (isPasswordMatched)
                {
                    ModelState.AddModelError("ResetPasswordViewModel.Password", "You cannot reuse passwords that you've used in your last two password changes. Please choose a new and unique password.");
                    return Page();
                }

                var result = await userManager.ResetPasswordAsync(user!, token!, ResetPasswordViewModel.Password);
                if (result.Succeeded)
                {
                    var hash = new PasswordHasher<Member>();
                    await passwordHistoryService.AddPasswordHistory(user.Id, hash.HashPassword(user, ResetPasswordViewModel.Password));
                    await userManager.UpdateSecurityStampAsync(user!);
                    TempData["Alert"] = "New password successfully changed";
                }
            }
            return Page();
        }
    }
}
