using AuthService.Data.DTOs;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // POST: api/Account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomRegisterRequest model)
        {
            try
            {
                var result = await _accountService.RegisterAsync(model);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                // In production, consider logging the exception details.
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/Account/confirmemail?userId=...
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            try
            {
                var result = await _accountService.ConfirmEmailAsync(userId, token);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/Account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var token = await _accountService.LoginAsync(model);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/Account/forgotpassword
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            try
            {
                await _accountService.ForgotPasswordAsync(model);
                return Ok(new { message = "If the email exists, a password reset link has been sent." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/Account/resetpassword
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            try
            {
                await _accountService.ResetPasswordAsync(model);
                return Ok(new { message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
