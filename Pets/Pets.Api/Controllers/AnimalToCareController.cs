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

        public AnimalToCareController(IAnimalToCareService animalToCareService)
        {
            _animalToCareService = animalToCareService;
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

        [HttpPost("add/{animalId}")]
        public async Task<IActionResult> Post(Guid animalId, [FromBody]CreateAnimalToCare request)
        {
            if(request == null)
            {
                return NotFound();
            }

            await _animalToCareService.AddAsync(animalId, request.DateFrom, request.DateTo);

            return NoContent();
        }

        [HttpPost("care/{animalId}")]
        public async Task<IActionResult> Post(Guid animalId)
        {
            //Getting care of animal by the user - send POST request.
            await Task.CompletedTask;

            return NoContent();
        }
    }
}
