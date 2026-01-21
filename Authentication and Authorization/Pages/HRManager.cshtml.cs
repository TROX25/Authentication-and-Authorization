using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_and_Authorization.Pages
{
    [Authorize(Roles = "HRManager")]
    public class HRManagerModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
