using ApplicationSecurityAssignment.Models;
using ApplicationSecurityAssignment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ApplicationSecurityAssignment.Pages
{
    public class IndexModel : PageModel
    {
        private SignInManager<Member> signInManager { get; }
        private UserManager<Member> userManager { get; }
        public Member CurrentMember { get; set; }
        public string PhotoBase64 { get; set; }
        public string DecryptedCreditCardNumber { get; set; }
        private IEncryptionService encryptionService { get; }
        public AuditLogService auditLogService { get; }


        public IndexModel(SignInManager<Member> _signInManager, UserManager<Member> _userManager, IEncryptionService _encryptionService, AuditLogService _auditLogService)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            encryptionService = _encryptionService;
            userManager = _userManager;
            auditLogService = _auditLogService;
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null) await auditLogService.LogLogoutAsync(user.Id);
            await signInManager.SignOutAsync();
            return RedirectToPage("Login");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                CurrentMember = user;
                PhotoBase64 = Convert.ToBase64String(user.Photo);
                DecryptedCreditCardNumber = encryptionService.Decrypt(user.CreditCardNumber);
            }
            return Page();
        }
    }
}
