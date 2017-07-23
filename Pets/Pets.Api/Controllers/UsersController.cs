using Microsoft.AspNetCore.Mvc;
using Pets.Infrastructure.Commands.Users;
using Pets.Infrastructure.DTO;
using Pets.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pets.Api.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            return await _userService.GetAllAsync();
        }
        
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetAsync(email);

            return Json(user);
        }

        [HttpGet("{email}/animals_to_care")]
        public async Task<IEnumerable<AnimalToCareDto>> GetAnimalsCaringByUser(string email)
        {
            return await _userService.GetCaringAnimalsAsync(email);
        }

        [HttpGet("{email}/animals_to_care/archive")]
        public async Task<IEnumerable<AnimalToCareDto>> GetAnimalsCaringByUserArchive(string email)
        {
            return await _userService.GetCaringAnimalsArchiveAsync(email);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LogInUser request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            if (await _userService.LoginAsync(request.Email, request.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, request.Email)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.Authentication.SignInAsync("CookieAuthentication", principal);

                return StatusCode(302);
            }

            return BadRequest();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
	        await HttpContext.Authentication.SignOutAsync("CookieAuthentication");
            return Ok();
	        //return Redirect("/users/login");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUser request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            await _userService.RegisterAsync(request.Email, request.FirstName, request.LastName, request.Password);

            return Created($"users/{request.Email}", new object());
        }
        
        [HttpPut("{email}")]
        public async Task<IActionResult> Put(string email, [FromBody]UpdateUser request)
        {
            if(request == null)
            {
                return BadRequest();
            }

            if (await DoesUserExist(email) == false)
            {
                return NotFound();
            }
            
            await _userService.UpdateAsync(request.Email, request.FirstName, request.LastName, request.Password);
            
            return NoContent();
        }
        
        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            if (await DoesUserExist(email) == false)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(email);

            return NoContent();
        }

        private async Task<bool> DoesUserExist(string email)
        {
            var user = await _userService.GetAsync(email);

            return user == null ? false : true;
        }
    }
}
