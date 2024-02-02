using ApplicationSecurityAssignment.Models;
using ApplicationSecurityAssignment.Services;
using ApplicationSecurityAssignment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace ApplicationSecurityAssignment.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> logger;
        private readonly IHttpContextAccessor context;
        private UserManager<Member> userManager { get; }
        private SignInManager<Member> signInManager { get; }
        private IEncryptionService encryptionService { get; }
        public PasswordHistoryService passwordHistoryService { get; }
        public AuditLogService auditLogService { get; }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<Member> _userManager, SignInManager<Member> _signInManager, ILogger<RegisterModel> _logger, IEncryptionService _encryptionService, IHttpContextAccessor httpContextAccessor, PasswordHistoryService _passwordHistoryService, AuditLogService _auditLogService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            encryptionService = _encryptionService;
            logger = _logger;
            context = httpContextAccessor;
            auditLogService = _auditLogService;
            passwordHistoryService = _passwordHistoryService;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(RegisterViewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("RegisterViewModel.Email", "Email is already in use.");
                    return Page();
                }


                // Encrypt credit card number
                var encryptedCreditCardNumber = encryptionService.Encrypt(RegisterViewModel.CreditCard.Replace(" ", ""));
                // Create member
                var member = new Member
                {
                    UserName = RegisterViewModel.Email,
                    Name = RegisterViewModel.Name,
                    Email = RegisterViewModel.Email,
                    AboutMe = RegisterViewModel.AboutMe,
                    Address = RegisterViewModel.Address,
                    Gender = RegisterViewModel.Gender,
                    CreditCardNumber = encryptedCreditCardNumber,
                    PhoneNumber = RegisterViewModel.PhoneNumber.Replace(" ", ""),
                    Photo = await ConvertImageToByteArray(RegisterViewModel.Photo)
                };

                var result = await userManager.CreateAsync(member, RegisterViewModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(member, isPersistent: false);
                    await auditLogService.LogLoginAsync(member.Id);

                    var hash = new PasswordHasher<Member>();
                    var hashedPassword = hash.HashPassword(member, RegisterViewModel.Password);
                    await passwordHistoryService.AddPasswordHistory(member.Id, hashedPassword);

                    return RedirectToPage("Index");
                }
                else
                {
                    logger.LogError("User creation failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            return Page();
        }

        private async Task<byte[]> ConvertImageToByteArray(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
