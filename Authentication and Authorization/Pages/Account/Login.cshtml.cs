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

                // claims opisuja informacje i uprawnienia (Name/Role) uzytkownikow a NIE SEKRETY, dlatego nie zapisuje tu hasla
                // Jeœli potrzebuje to moge zrobic swoje Claimy -> new Claim("UserId", user.Id.ToString()) albo new Claim("Department", "IT")

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@gmail.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2025-01-01")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Cridential.RememberMe // jezeli uzytkownik zaznaczyl "Remember Me" to cookie bedzie trwalsze
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

                return RedirectToPage("/Index");
            }
            return Page();
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

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
