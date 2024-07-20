using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Domain;
using NOLA_API.DTOs;
using NOLA_API.Infrastructure.Messages;
using NOLA_API.Interfaces;
using NOLA_API.Services;
using static System.Net.Mime.MediaTypeNames;

namespace NOLA_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser(UserDto userDto)
        {
            var user = (await GetCurrentUser()).Value;
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            if (user == null) return NotFound();
            if(!string.IsNullOrEmpty(userDto.UserName))appUser!.UserName = userDto.UserName;
            if(!string.IsNullOrEmpty(userDto.Email))appUser!.Email = userDto.Email;
            if(!string.IsNullOrEmpty(userDto.Image))appUser!.Image = userDto.Image;
            if (!string.IsNullOrEmpty(userDto.Description)) appUser!.Description = userDto.Description;
            if (!string.IsNullOrEmpty(userDto.EntityName)) appUser!.EntityName = userDto.EntityName;
            //if(userDto.Links != null)appUser!.Links = userDto.Links;


            var result = await _userManager.UpdateAsync(appUser);
   
            if (result.Succeeded)
            {
                return CreateUserObject(appUser);
            }

            return BadRequest("Problem updating the user");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email is already taken!");
                return ValidationProblem();
            }

            string initials = GetInitials(registerDto.EntityName).ToUpper();
            var avatarImage = Avatar(initials);
            var base64String = string.Empty;


            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {            
                avatarImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] bytes = stream.ToArray();
                base64String = System.Convert.ToBase64String(bytes);
            }




            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email.Split("@")[0].ToLower(),
                EntityName = registerDto.EntityName,
                Image = base64String
            };


            try
            {
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action(
                        nameof(ConfirmEmail),
                        "Account", 
                        new { email = user.Email, token = token },
                        Request.Scheme);

                    Message message = new Message();
                    message.Subject = "Registration at NOLA";
                    message.Content = $"<p>You’re receiving this message because you recently signed up for a NOLA account. Please confirm your email address by clicking the link below: <a href='{confirmationLink}'>confirm</a></p>";

                    await TrySendEmailAsync(user.Email, message);

                    return CreateUserObject(user);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("Mail is not registered in the system. Please try again.");
            }
                
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok("Email successfully confirmed.");
            }

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("Mail is not registered in the system. Please try again.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action(
                nameof(ResetPassword),
                "Account",
                new { email = email, token = token },
                Request.Scheme);

            Message message = new Message();
            message.Subject = "Password Reset Instructions For NOLA Account";
            message.Content = $"<p>Hello {user.UserName}</p><p>Click the link to reset your password for your NOLA account: <a href='{callbackUrl}'>reset password</a></p>";

            try
            {
                await _emailService.SendAsync(user.Email, message);

                return Ok("Password Reset Instructions have been sent by Email.");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return BadRequest("An error occurred while sending the email. Please try again later.");
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Password not correct");
            }

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return BadRequest("Can not find user for change password");
            }


            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(resetPassResult.Errors);
            }
            return Ok("Password was changed");
        }

        [HttpGet("reset-password")]
        [AllowAnonymous]
        public ActionResult<ResetPasswordDto> ResetPassword(string email, string token)
        {
            return new ResetPasswordDto { Token = token, Email = email };
        }

        private ActionResult<UserDto> CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                Email = user.Email,
                Image = user.Image,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                Description = user.Description,
                EntityName = user.EntityName
            };
        }

        private async Task TrySendEmailAsync(string email, Message message)
        {
            try
            {
                await _emailService.SendAsync(email, message);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }



        private System.Drawing.Image Avatar(string nameInput)
        {
            using (var bitmap = new Bitmap(50, 50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    using (Brush b = new SolidBrush(Color.SkyBlue))
                    {

                        g.FillEllipse(b, 0, 0, 49, 49);
                    }

                    float emSize = 12;
                    g.DrawString(nameInput, new System.Drawing.Font(FontFamily.GenericSansSerif, emSize, FontStyle.Regular),
                        new SolidBrush(Color.White), 10, 15);
                }

                using (var memStream = new System.IO.MemoryStream())
                {
                    bitmap.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);
                    return System.Drawing.Image.FromStream(memStream);
                }
            }
        }


        private string GetInitials(string entityName)
        {
            string initials = string.Empty;


            if (entityName.Contains(' '))
            {
                foreach (var item in entityName.Split(' '))
                {
                    if (initials.Length < 2)
                    {
                        initials += item[0];
                    }
                }
            }
            else
            {
                initials = entityName[0].ToString() + entityName[1].ToString();
            }



            return initials;

        }
    }
}