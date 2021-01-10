

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Auth;
using Identity.Models;
using Newtonsoft.Json;

namespace Identity.Helpers
{
    public class Tokens
    {
        public static async Task<JwtToken> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions)
        {
            var response = new JwtToken
            {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                Token = await jwtFactory.GenerateEncodedToken(userName, identity),
                ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return response;
        }
    }

    public class JwtToken
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
