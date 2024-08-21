using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Database;
using MyFace.Repositories;
using MyFace.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/login")]
    public class JWTLoginController : ControllerBase
    {
        private readonly IUsersRepo _usersRepo;
        private readonly IJWTService _jWTService;

        public JWTLoginController(IUsersRepo usersRepo, IJWTService jWTService)
        {
            _usersRepo = usersRepo;
            _jWTService = jWTService;

        }


        [HttpPost("/login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginUserRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized();
            }

            var user = _usersRepo.Authenticate(loginRequest.Username, loginRequest.Password);
            if (user is null)
            {
                return Unauthorized();
            }

            string token = _jWTService.GenerateToken(user.Id);

            return Accepted(new JWTLoginUserResponse(user, token));
        }

    }
}