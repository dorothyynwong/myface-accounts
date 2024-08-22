using System;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;


namespace MyFace.Helpers
{
    public static class AuthorizationHelper
    {
        public static (string, string) GetUserAndPasswordAuthorizationHeader(HttpRequest request)
        {
            var authHeader = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            var username = credentials[0];
            var password = credentials[1];

            return (username, password);
        }

        public static string GetCurrentToken(HttpRequest request)
        {
            var authHeader = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);
            // var bearerBytes = Convert.FromBase64String(authHeader.Parameter);
            // var bearer = Encoding.UTF8.GetString(bearerBytes);

            return authHeader.Parameter;
        }
    }
}