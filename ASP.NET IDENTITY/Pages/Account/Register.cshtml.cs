using ASP.NET_IDENTITY.Data;
using ASP.NET_IDENTITY.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace ASP.NET_IDENTITY.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IEmailService emailService;

        public RegisterModel(UserManager<User> userManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; } = new RegisterViewModel();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = new User
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email,
            };

            var claimDepartment = new Claim("Department", RegisterViewModel.Department);
            var claimPosition = new Claim("Position", RegisterViewModel.Position);

            var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (!result.Succeeded)
            {
                this.userManager.AddClaimAsync(user, claimDepartment).Wait();
                this.userManager.AddClaimAsync(user, claimPosition).Wait();

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            else
            {
                // Tworzę mechanizm potwierdzenia adresu email przy rejestracji
                var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                // Wygeneruj link do potwierdzenia emaila i wyślij go do użytkownika (np. przez email)
                var confirmationLink = Url.PageLink("/Account/ConfirmEmail",  
                    values: new { userId = user.Id, token });
                // SMTP SERWER -> przeniesiono do EmailService

                await emailService.SendEmailAsync("pimalogik@gmail.com",
                    user.Email,
                     "Potwierdzenie rejestracji",
                     $"Kliknij w link, aby potwierdzić rejestrację: {confirmationLink}");



                return RedirectToPage("/Account/Login");
            }

        }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DisplayName("Department")]
        public string Department { get; set; } = string.Empty;
        [Required]
        [DisplayName("Position")]
        public string Position { get; set; } = string.Empty;
    }


}
