using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        // Dzieki temu moge pobrac konfiguracje z appsettings.json
        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // JWT zawiera Claims, Signing Credentials i informacje o wygaśnięciu tokenu
        [HttpPost] // Auth: JWT
        public IActionResult Authenticate([FromBody]Credential credential)
        {
            if (credential.UserName == "admin" && credential.Password == "admin")
            {
                //zalogowano

                // claims opisuja informacje i uprawnienia (Name/Role) uzytkownikow a NIE SEKRETY, dlatego nie zapisuje tu hasla
                // Jeśli potrzebuje to moge zrobic swoje Claimy -> new Claim("UserId", user.Id.ToString()) albo new Claim("Department", "IT")

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@gmail.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2025-01-01")
                };

                var expiresAt = DateTime.UtcNow.AddMinutes(30);

                return Ok(new
                {
                    access_token = CreateToken(claims, expiresAt),
                    expires_at = expiresAt,
                });
            }
            else
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized to access this resource.");
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Title = "Authentication Failed",
                    Detail = "Invalid username or password."
                };
                return Unauthorized(problemDetails);

            }
        }

        private object CreateToken(List<Claim> claims, DateTime expiresAt)
        {
            var claimsDic = new Dictionary<string, object>();
            if (claims is not null && claims.Count > 0)
            {
                foreach (var claim in claims)
                {
                    claimsDic.Add(claim.Type, claim.Value);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["SecretKey"]?? throw new InvalidOperationException("SecretKey is missing");)),
                    SecurityAlgorithms.HmacSha256Signature),
                Claims = claimsDic,
                Expires = expiresAt,
                NotBefore = DateTime.UtcNow

            };

            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
