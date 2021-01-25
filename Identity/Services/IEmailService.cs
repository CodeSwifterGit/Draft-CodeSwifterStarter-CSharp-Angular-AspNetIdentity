using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Services
{
    public interface IEmailService
    {
        void SendRegistrationEmail(string email, string userId, string token);
        void SendResetPasswordEmail(string email, string userId, string token);
    }
}
