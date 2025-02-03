using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.DTOs
{
    public class CustomRegisterRequest
    {
        [EmailAddress]
        [Required]
        public required string Email { get; init; }
        [Required]
        public required string Password { get; init; }
        public required string FullName { get; init; }
    }
}
