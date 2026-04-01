using ASP.NET_IDENTITY.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;

namespace ASP.NET_IDENTITY.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public string Message { get; set; } = string.Empty;

        public ConfirmEmailModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    Message = "Email confirmed successfully. You can now log in.";
                    return Page();
                }
            }
            Message = "Email confirmation failed. Please try again.";
            return Page();
        }
    }
}
