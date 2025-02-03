using AuthService.Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Data.DTOs;
using AuthService.Services.Interfaces;

namespace AuthService.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(CustomRegisterRequest model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                FullName = model.FullName,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            if (user == null)
            {
                throw new Exception("This user does not exist.");
            }
            await _userManager.AddToRoleAsync(user, "USER");
           
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://innoshop.pl/confirmEmail?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            return $"Registration successful. token: {token} userid: {user.Id}";
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("This user does not exist.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new Exception("Registration is not confirmed.");
            }

            return "Email confirmed.";
        }

        public async Task<string> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new Exception("Incorrect email or password.");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new Exception("Email is not confirmed.");
            }

            if (!user.IsActive)
            {
                throw new Exception("This account is not active.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                throw new Exception("Incorrect email or password.");
            }

            var token = await GenerateJwtToken(user);
            return token;
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new Exception("This user does not exists.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://innoshop.pl/reset-password?token={Uri.EscapeDataString(resetToken)}&email={user.Email}";

            /* await _emailSender.SendEmailAsync(
                 user.Email,
                 "Reset hasła",
                 $"Twój link do resetu hasła: {resetLink}");*/
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new Exception("Użytkownik nie istnieje.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.ResetCode, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expiresInMinutes = jwtSection.GetValue<int>("ExpiresInMinutes");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim("fullName", user.FullName ?? "")
        };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
