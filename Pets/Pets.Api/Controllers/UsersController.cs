using Microsoft.AspNetCore.Mvc;
using Pets.Infrastructure.Commands.Users;
using Pets.Infrastructure.DTO;
using Pets.Infrastructure.Services;
using System;
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

        //[HttpGet("{email}/animals_to_care/archive")]
        //public async Task<IEnumerable<AnimalToCareDto>> GetAnimalsCaringByUserArchive(string email)
        //{
        //    return await _userService.GetCaringAnimalsAsync(email);
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUser request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            try
            {
                await _userService.RegisterAsync(request.Email, request.FirstName, request.LastName, request.Password);
            }
            catch(Exception e)
            {
                return StatusCode(409, e.Message);
            }
            
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

            try
            {
                await _userService.UpdateAsync(request.Email, request.FirstName, request.LastName, request.Password);
            }
            catch (Exception e)
            {
                return StatusCode(409, e.Message);
            }

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
