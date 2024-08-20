using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Database;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUsersRepo _usersRepo;

        public LoginController(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
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

            return Accepted(new LoginUserResponse(user));
        }

    }
}