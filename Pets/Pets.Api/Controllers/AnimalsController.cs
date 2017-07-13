using Microsoft.AspNetCore.Mvc;
using Pets.Infrastructure.Commands.Animals;
using Pets.Infrastructure.DTO;
using Pets.Infrastructure.Services;
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

        [HttpPost]
        public async Task<IActionResult> Post(string email, [FromBody]CreateAnimal request)
        {
            await _animalService.AddAsync(email, request.Name, request.YearOfBirth);

            return NoContent();
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string email, string name, [FromBody]UpdateAnimal request)
        {
            await _animalService.UpdateAsync(email, name, request.Name, request.YearOfBirth);

            return NoContent();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string email, string name)
        {
            await _animalService.DeleteAsync(email, name);

            return NoContent();
        }
    }
}
