using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pets.Infrastructure.Commands.Animals;
using Pets.Infrastructure.DTO;
using Pets.Infrastructure.Exceptions;
using Pets.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Api.Controllers
{
    [Route("users/{email}/[controller]")]
    public class AnimalsController : Controller
    {
        private readonly IAnimalService _animalService;
        private readonly IUserService _userService;

        public AnimalsController(IAnimalService animalService, IUserService userService)
        {
            _animalService = animalService;
            _userService = userService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string email, string name)
        {
            var animal = await _animalService.GetAsync(email, name);
            if(animal == null)
            {
                return NotFound();
            }

            return Json(animal);
        }

        [HttpGet]
        public async Task<IEnumerable<AnimalDto>> Get(string email)
        {
            return await _animalService.GetAllAsync(email);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string email, [FromBody]CreateAnimal request)
        {
            if (email != await GetLoggedUserEmail())
            {
                throw new ServiceException(ErrorCodes.AnimalNotAvailable, "You can only add animals to your account.");
            }

            await _animalService.AddAsync(email, request.Name, request.YearOfBirth);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string email, string name, [FromBody]UpdateAnimal request)
        {
            if (email != await GetLoggedUserEmail())
            {
                throw new ServiceException(ErrorCodes.AnimalNotAvailable, "You can only edit animals in your account.");
            }

            await _animalService.UpdateAsync(email, name, request.Name, request.YearOfBirth);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string email, string name)
        {
            if (email != await GetLoggedUserEmail())
            {
                throw new ServiceException(ErrorCodes.AnimalNotAvailable, "You can only delete animals in your account.");
            }

            await _animalService.DeleteAsync(email, name);

            return NoContent();
        }

        public async Task<string> GetLoggedUserEmail()
        {
            if (HttpContext.User.Identity.Name == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, "There is no logged in user.");
            }

            return await Task.FromResult(HttpContext.User.Identity.Name);
        }
    }
}
