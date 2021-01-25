using Identity.Database;
using Identity.Helpers;
using Identity.Models;
using Identity.Services;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        private UserManager<AppUser> _userManager;
        private DatabaseContext _dbContext;
        private IConfiguration _configuration;
        private IEmailService _emailService;

        public AccountsController(
            UserManager<AppUser> userManager, 
            DatabaseContext databaseContext, 
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userManager = userManager;
            _dbContext = databaseContext;
            _configuration = configuration;
            _emailService = emailService;
        }

        // POST: AccountsController/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = model.ToUser();

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            var emailConfimrationSettings = _configuration.GetSection("EmailConfirmation").Get<EmailConfirmation>();
            if (emailConfimrationSettings.Enabled)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _emailService.SendRegistrationEmail(user.Email, user.Id, token);
            }

            return new OkResult();
        }

        [HttpPost]
        [Authorize(Policy = "ApiUser")]
        public async Task<ActionResult> SendForgotPasswordEmail()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var emailConfimrationSettings = _configuration.GetSection("EmailConfirmation").Get<EmailConfirmation>();
            if (emailConfimrationSettings.Enabled)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                _emailService.SendResetPasswordEmail(user.Email, user.Id, token);
            }

            return new OkResult();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            return new OkObjectResult(result);
        }

        [HttpGet]
        [Route("confirm")]
        public async Task<ActionResult> ConfirmAccount(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return new OkObjectResult(result);

        }
    }
}
