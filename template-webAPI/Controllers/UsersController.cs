using template_webApi.Data;
using template_webApi.Models;
using template_webApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace template_webApi.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public UsersController(UserManager<ApplicationUser> userManager,
                               JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AuthenticationResponseModel>> Login(AuthenticationRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Wrong login details");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return BadRequest("Wrong login details");
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {
                return BadRequest("Wrong login details");
            }

            var token = _jwtService.CreateToken(user);

            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<AuthenticationResponseModel>> Register(RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Wrong register details");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Wrong confirm password");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                return BadRequest("Account with this username already exists");
            }

            user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                return BadRequest("Account with this email already exists");
            }

            try
            {
                user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var hasher = new PasswordHasher<ApplicationUser>();
                var hashed = hasher.HashPassword(user, model.Password);

                user.PasswordHash = hashed;

                var result = await _userManager.CreateAsync(user);

                result = await _userManager.AddToRoleAsync(user, "User");

                return _jwtService.CreateToken(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
