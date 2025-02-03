using Microsoft.AspNetCore.Identity;

namespace AuthService.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public bool IsActive { get; set; } = true;
        public required string FullName { get; set; }
    }
}
