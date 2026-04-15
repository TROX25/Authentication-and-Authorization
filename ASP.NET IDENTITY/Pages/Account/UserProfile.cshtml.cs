using ASP.NET_IDENTITY.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_IDENTITY.Pages.Account
{
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> userManager;

        public UserProfileModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet()
        {
        }
    }
}
