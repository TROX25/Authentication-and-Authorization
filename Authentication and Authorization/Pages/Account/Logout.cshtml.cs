using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Authentication_and_Authorization.Constants.AuthSchemes;

namespace Authentication_and_Authorization.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync(MyCookie);
            return RedirectToPage("/Index");
        }

    }
}
