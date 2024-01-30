using ApplicationSecurityAssignment.Models;
using ApplicationSecurityAssignment.Services;
using ApplicationSecurityAssignment.ViewModels;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationSecurityAssignment.Pages
{
    [ValidateReCaptcha]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }
        private SignInManager<Member> signInManager { get; }
        private UserManager<Member> userManager { get; }
        public AuditLogService auditLogService { get; }

        public LoginModel(SignInManager<Member> _signInManager, AuditLogService _auditLogService, UserManager<Member> _userManager)
        {
            signInManager = _signInManager;
            auditLogService = _auditLogService;
            userManager = _userManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(LoginViewModel.Email, LoginViewModel.Password, false, true);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(LoginViewModel.Email);
                    if (user != null)
                    {
                        await auditLogService.LogLoginAsync(user.Id);
                        TempData["SuccessMessage"] = "Login Successful";
                        return RedirectToPage("Index");
                    }
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("LoginViewModel.Email", "This account has been locked out. Please try again later.");
                }
                else
                {
                    ModelState.AddModelError("LoginViewModel.Email", "Email or password is incorrect. Please try again.");
                    ModelState.AddModelError("LoginViewModel.Password", "Email or password is incorrect. Please try again.");
                }
            }
            return Page();
        }
    }
}
