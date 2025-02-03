using AuthService.Data.DTOs;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task ForgotPasswordAsync(ForgotPasswordRequest model);
        Task<string> LoginAsync(LoginRequest model);
        Task<string> RegisterAsync(CustomRegisterRequest model);
        Task ResetPasswordAsync(ResetPasswordRequest model);
    }
}