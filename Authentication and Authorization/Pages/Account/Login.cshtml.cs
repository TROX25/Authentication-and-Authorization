using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Authentication_and_Authorization.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Cridential Cridential { get; set; } = new Cridential();   



        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            // jesli postawie tu brakepoint i watch wpisze this.Cridential w watch to zobacze wartosci wpisane w formularzu (haslo i login)

            if (!ModelState.IsValid) return Page();

            if(Cridential.UserName == "admin" && Cridential.Password == "admin")
            {
                //zalogowano
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@gmail.com")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToPage("/Index");
            }
            else
            {
                return Page();
            }

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
