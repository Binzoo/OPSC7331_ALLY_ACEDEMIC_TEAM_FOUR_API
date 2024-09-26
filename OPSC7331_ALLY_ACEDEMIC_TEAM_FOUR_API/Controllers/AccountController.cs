using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Data;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailService;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AppDbContext context, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _emailService = emailSender;
        }

        [HttpPost("register-studnet")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new AppUser { Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, College = model.College, Degree = model.Degree };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "student");
                return Ok(new { message = "User registered successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO model)
        {
            var user = new AppUser { Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, College = model.College, Degree = model.Degree };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "ADMIN");
                return Ok(new { message = "Admin registered successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login-studnet")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256));

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized();
        }


        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                {
                    return Ok(new { message = "Role added successfully" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Role already exists");
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswrodDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok("If an account with your email was found, we've sent a code to it.");
            }
            var code = new Random().Next(100000, 999999).ToString();

            var resetEntry = new PasswordReset
            {
                UserId = user.Id,
                Code = code,
                //code expires in 15 seconds
                ExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };

            _context.PasswordResetsCodes.Add(resetEntry);
            await _context.SaveChangesAsync();

            await SendPasswordResetCodeByEmail(user.Email!, code);
            return Ok("Password rest code sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RestPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email!);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var isValidCode = await ValidateResetCodeAsync(user.Id, model.Code!);
            if (!isValidCode)
            {
                return BadRequest("Invalid or Expired Code");
            }

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                return BadRequest(removePasswordResult.Errors);
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword!);
            if (!addPasswordResult.Succeeded)
            {
                return BadRequest(addPasswordResult.Errors);
            }

            return Ok("Password has been reset successfully.");
        }

        private async Task<bool> ValidateResetCodeAsync(string userId, string submittedCode)
        {
            // Retrieve and validate the reset code from your database
            var resetEntry = await _context.PasswordResetsCodes.FirstOrDefaultAsync(p => p.UserId == userId && p.Code == submittedCode);
            if (resetEntry != null && resetEntry.ExpiryTime > DateTime.UtcNow)
            {
                //remove or invalidate the code
                _context.PasswordResetsCodes.Remove(resetEntry);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private async Task SendPasswordResetCodeByEmail(string email, string code)
        {
            string subject = "Your Password Reset Code";
            string message = $"Your password reset code is: {code}. This code will expire in 15 minutes.";
            await _emailService.SendEmailAsync(email, subject, message);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userEmail!);
            return Ok(user);
        }

        [Authorize]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userEmail!);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest("Error updating password.");
                }
            }
            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("User updated successfully.");
            }
            return BadRequest("Error updating user.");
        }



    }
}
