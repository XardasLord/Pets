using Microsoft.AspNetCore.Mvc;
using Pets.Infrastructure.Commands.Animals;
using Pets.Infrastructure.DTO;
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
            UserDto user = new UserDto();

            try
            {
                user = await _userService.GetAsync(email);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            var animal = await _animalService.GetAsync(user.Id, name);

            return Json(animal);
        }

        [HttpGet]
        public async Task<IEnumerable<AnimalDto>> Get(string email)
        {
            UserDto user = new UserDto();

            try
            {
                user = await _userService.GetAsync(email);
            }
            catch (Exception ex)
            {
                // Catch an error
            }

            return await _animalService.GetAllAsync(user.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string email, [FromBody]CreateAnimal request)
        {
            UserDto user = new UserDto();

            try
            {
                user = await _userService.GetAsync(email);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            await _animalService.AddAsync(user.Id, request.Name, request.YearOfBirth);

            return NoContent();
        }

    }
}
