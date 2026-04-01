using Microsoft.AspNetCore.Identity;

namespace ASP.NET_IDENTITY.Data
{
    public class User : IdentityUser
    {
        public string Department { get; set; }= string.Empty;
        public string Position { get; set; }= string.Empty;
    }
}
