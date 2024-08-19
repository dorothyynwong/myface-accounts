using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Database;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IUsersRepo _usersRepo;

        public LoginController(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public LoginUserResponse Login(LoginUserRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var user = _usersRepo.Authenticate(loginRequest.Username, loginRequest.Password);
                if (user is null)
                {
                    // return RedirectToAction("Index", "Home");
                    return null;
                }
                // ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                // return View(model);
                return new LoginUserResponse(user);
            }
            // return View(model);
            return null;

        }
    }
}