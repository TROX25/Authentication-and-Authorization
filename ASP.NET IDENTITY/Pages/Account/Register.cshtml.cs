using ASP.NET_IDENTITY.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace ASP.NET_IDENTITY.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmailService emailService;

        public RegisterModel(UserManager<IdentityUser> userManager, IEmailService emailService)
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
            // Tutaj dodaj logikę rejestracji użytkownika, np. zapis do bazy danych
            var user = new IdentityUser
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };

            var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (!result.Succeeded)
            {
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
    }


}
