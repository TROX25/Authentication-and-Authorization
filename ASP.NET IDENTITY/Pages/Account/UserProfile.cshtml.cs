using ASP.NET_IDENTITY.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ASP.NET_IDENTITY.Pages.Account
{
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public UserProfileViewModel UserProfile { get; set; }
        [BindProperty]
        public string? SuccessMessage { get; set; }

        public UserProfileModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.UserProfile = new UserProfileViewModel();
        }

        // Get odpowiada za pobranie aktualnych danych użytkownika i wyświetlenie ich w zakładce UserProfile
        public async Task<IActionResult> OnGetAsync()
        {
            this.SuccessMessage = string.Empty;
            var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();
            // w teorii zamiast tego Tuple mozna zrobic tak
            // var user = await GetUserAsync();
            // var departmentClaim = await GetDepartmentClaimAsync();
            // var positionClaim = await GetPositionClaimAsync();

            if (user != null)
            {
                this.UserProfile.Email = User.Identity?.Name ?? string.Empty;
                this.UserProfile.Department = departmentClaim?.Value ?? string.Empty;
                this.UserProfile.Position = positionClaim?.Value ?? string.Empty;
            }   
            return Page();
        }

        // Post odpowiada za aktualizację danych użytkownika, w tym przypadku działu i stanowiska jeśli zostaną one zmienione w zakładce UserProfile
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();
                if (user != null && departmentClaim != null)
                {
                    await userManager.ReplaceClaimAsync(user, departmentClaim, new Claim(departmentClaim.Type, UserProfile.Department ?? string.Empty));
                }
                if (user != null && positionClaim != null)
                {
                    await userManager.ReplaceClaimAsync(user, positionClaim, new Claim(positionClaim.Type, UserProfile.Position ?? string.Empty));
                }
            }
            catch 
            {
                ModelState.AddModelError("UserProfile", "An error occurred while updating your profile. Please try again.");
            }
            
            this.SuccessMessage = "The user profile has been updated successfully.";

            return Page();
            
        }

        // Tworzę metodę która zwraca user departmentclaim i postionclaim w jednym wywołaniu, dzięki temu unikam wielokrotnego pobierania tych samych danych z bazy danych, co jest bardziej efektywne.
        // Zamiast tego można by było zrobić trzy osobne metody GetUserAsync(), GetDepartmentClaimAsync() i GetPositionClaimAsync() ale wtedy trzeba by było trzykrotnie pobierać dane z bazy danych, co jest mniej efektywne
        private async Task<(User? user, Claim? departmentClaim, Claim? positionClaim)> GetUserInfoAsync()
        {
            var user = await userManager.FindByNameAsync(User.Identity?.Name ?? string.Empty);
            if (user != null)
            {
                var claims = await userManager.GetClaimsAsync(user);
                var departmentClaim = claims.FirstOrDefault(c => c.Type == "Department");
                var positionClaim = claims.FirstOrDefault(c => c.Type == "Position");
                return (user, departmentClaim, positionClaim);
            }
            else
            {
                return (null, null, null);
            }
        }

    }

    public class UserProfileViewModel
    {
        public string? Email { get; set; }
        [Required]
        public string? Department { get; set; }
        [Required]
        public string? Position { get; set; }
    }
}
