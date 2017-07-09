using Microsoft.AspNetCore.Mvc;
using Pets.Infrastructure.Commands.Users;
using Pets.Infrastructure.DTO;
using Pets.Infrastructure.Services;
using System.Collections.Generic;
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
        public async Task<IEnumerable<UserDto>> Get()
        {
            return await _userService.GetAllAsync();
        }
        
        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var user = await _userService.GetAsync(email);
            if(user == null)
            {
                return NotFound();
            }

            return Json(user);
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
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
