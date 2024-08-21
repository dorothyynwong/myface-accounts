using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;
using MyFace.Helpers;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepo _users;

        public UsersController(IUsersRepo users)
        {
            _users = users;
        }
        
        [HttpGet("")]
        public ActionResult<UserListResponse> Search([FromQuery] UserSearchRequest searchRequest)
        {
            var users = _users.Search(searchRequest);
            var userCount = _users.Count(searchRequest);
            return UserListResponse.Create(searchRequest, users, userCount);
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetById([FromRoute] int id)
        {
            var user = _users.GetById(id);
            return new UserResponse(user);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateUserRequest newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = _users.Create(newUser);

            var url = Url.Action("GetById", new { id = user.Id });
            var responseViewModel = new UserResponse(user);
            return Created(url, responseViewModel);
        }

        [HttpPatch("{id}/update")]
        [Authorize]
        public ActionResult<UserResponse> Update([FromRoute] int id, [FromBody] UpdateUserRequest update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _users.Update(id, update);
            return new UserResponse(user);
        }
        
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete([FromRoute] int id)
        {
            
            string authorizationString = Request.Headers["Authorization"];
            string credentials = authorizationString.Split(" ")[1];
            var credentialBytes = Convert.FromBase64String(credentials);
            string[] decodedCredentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);

            var user = _users.Authenticate(decodedCredentials[0], decodedCredentials[1]);
            if (user == null) return Unauthorized("Invalid user");

            if (user.Role != Role.ADMIN)
                return Forbid();
                
            _users.Delete(id);
            return Ok();
        }
    }
}