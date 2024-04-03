using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Domain;
using NOLA_API.DTOs;
using NOLA_API.Services;

namespace NOLA_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email is already taken!");
                return ValidationProblem();
            }
            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email.Split("@")[0].ToLower()
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
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

        private ActionResult<UserDto> CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                Email = user.Email,
                Image = string.Empty,
                DisplayName = string.Empty,
                Username = string.Empty,
                Token = _tokenService.CreateToken(user),
            };
        }
    }
}