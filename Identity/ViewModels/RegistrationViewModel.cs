using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class RegistrationViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AppUser ToUser()
        {
            return new AppUser
            {
                Email = Email,
                UserName = Email,
                FirstName = FirstName,
                LastName = LastName,
            };
        }
    }
}
