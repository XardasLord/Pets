using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pets.Infrastructure.Commands.AnimalToCare;
using Pets.Infrastructure.DTO;
using Pets.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Api.Controllers
{
    [Route("animals_to_care")]
    public class AnimalToCareController : Controller
    {
        private readonly IAnimalToCareService _animalToCareService;
        private readonly IAnimalService _animalService;

        public AnimalToCareController(IAnimalToCareService animalToCareService,
            IAnimalService animalService)
        {
            _animalToCareService = animalToCareService;
            _animalService = animalService;
        }

        [HttpGet]
        public async Task<IEnumerable<AnimalToCareDto>> GetAllActive()
        {
            return await _animalToCareService.GetAllActiveAsync();
        }

        [HttpGet("archive")]
        public async Task<IEnumerable<AnimalToCareDto>> GetAllArchive()
        {
            return await _animalToCareService.GetAllArchiveAsync();
        }

        [HttpGet("{animalId}")]
        public async Task<IEnumerable<AnimalToCareDto>> GetByAnimalId(Guid animalId)
        {
            return await _animalToCareService.GetAsync(animalId);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Post([FromBody]CreateAnimalToCare request)
        {
            if(request == null)
            {
                return NotFound();
            }

            await _animalToCareService.AddToCareListAsync(request.AnimalId, request.DateFrom, request.DateTo);

            return NoContent();
        }

        [Authorize]
        [HttpPost("care")]
        public async Task<IActionResult> Post([FromBody]GetAnimalToCare request)
        {
            if(request == null)
            {
                return NotFound();
            }

            //TODO: Getting care of animal by the user - send POST request.
            await _animalToCareService.GetAnimalToCareAsync(request.AnimalId, request.UserId);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{animalId}")]
        public async Task<IActionResult> Put(Guid animalId, [FromBody]UpdateAnimalToCare request)
        {
            var animal = await _animalService.GetAsync(animalId);

            if (animal.User.Email != await GetLoggedUserEmail())
            {
                throw new Exception("You can only edit animal to care information from your account.");
            }

            await _animalToCareService.UpdateAsync(animalId, request.DateFrom, request.DateTo, request.IsTaken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{animalId}")]
        public async Task<IActionResult> Delete(Guid animalId)
        {
            var animal = await _animalService.GetAsync(animalId);
            
            if (animal.User.Email != await GetLoggedUserEmail())
            {
                throw new Exception("You can only delete animal to care from your account.");
            }

            await _animalToCareService.DeleteAsync(animalId);

            return NoContent();
        }

        public async Task<string> GetLoggedUserEmail()
        {
            if (HttpContext.User.Identity.Name == null)
            {
                throw new System.Exception("There is no logged user.");
            }

            return await Task.FromResult(HttpContext.User.Identity.Name);
        }
    }
}
