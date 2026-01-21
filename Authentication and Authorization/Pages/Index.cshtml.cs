using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_and_Authorization.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // moge postawic brakepoint na wysokosci poczatku tej funkcji i w watch wpisac base.User aby otrzymac informacje dotyczace Identity/Claims.
            // dla szczegu³ów base.User.Identity

        }
        
    }
}
