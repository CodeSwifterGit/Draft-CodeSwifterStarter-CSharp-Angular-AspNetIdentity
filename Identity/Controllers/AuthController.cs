using Identity.Auth;
using Identity.Helpers;
using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private const string COOKIE_NAME = "auth-cookie";
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<AppUser> userManager, 
            IJwtFactory jwtFactory, 
            IOptions<JwtIssuerOptions> jwtOptions,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _configuration = configuration;
        }

        public JwtIssuerOptions JwtOptions => _jwtOptions;

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("InvalidUsernameOrPassword", "Invalid username or password.", ModelState));
            }

            if (_configuration.GetSection("EmailConfirmation").Get<EmailConfirmation>().Enabled)
            {
                var user = await _userManager.FindByNameAsync(credentials.Email);

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return BadRequest(Errors.AddErrorToModelState("EmailNotConfirmed", "Email not confirmed.", ModelState));
                }
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.Email, JwtOptions);

            Response.Cookies.Append(COOKIE_NAME, jwt.Token, new CookieOptions { 
                HttpOnly = true,
                Secure = true
            });

            return new OkObjectResult(JsonConvert.SerializeObject(jwt, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(COOKIE_NAME);

            return new OkResult();
        }

        [HttpGet("isloggedin")]
        public IActionResult IsLoggedIn()
        {
            var authCookie = Request.Cookies[COOKIE_NAME];

            return new OkObjectResult(!string.IsNullOrEmpty(authCookie));
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
