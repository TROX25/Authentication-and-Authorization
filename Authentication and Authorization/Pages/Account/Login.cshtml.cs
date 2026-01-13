using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Authentication_and_Authorization.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Cridential Cridential { get; set; } = new Cridential();   



        public void OnGet()
        {
        }
        public void OnPost()
        {
            // jesli postawie tu brakepoint i watch wpisze this.Cridential to zobacze wartosci wpisane w formularzu (haslo i login)

        }
    }

    public class Cridential
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
