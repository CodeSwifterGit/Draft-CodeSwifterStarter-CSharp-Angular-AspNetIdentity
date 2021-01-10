using Identity.Database;
using Identity.Helpers;
using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountsController(UserManager<AppUser> userManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _dbContext = databaseContext;
        }

        public ActionResult Get()
        {
            return Ok("Ok");
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

            return new OkResult();
        }
    }
}
