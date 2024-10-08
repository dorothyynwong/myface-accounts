using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFace.Helpers;
using MyFace.Models.Database;
using MyFace.Repositories;

namespace MyFace.Services

{

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>

    {
        private readonly IUsersRepo _usersRepo;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUsersRepo usersRepo)
            : base(options, logger, encoder, clock)
        {

            // _usersService = userService;
            _usersRepo = usersRepo;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()

        {

            if(!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            User user = null;

            try
            {

                (string username, string password) = AuthorizationHelper.GetUserAndPasswordAuthorizationHeader(Request);
                user = _usersRepo.Authenticate(username, password);
            }

            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (user == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new[] {

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
            //throw new NotImplementedException();

        }
    }
}