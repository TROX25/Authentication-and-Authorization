using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_and_Authorization.Pages
{
    [Authorize(Policy = "MustBeAdmin")]
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
