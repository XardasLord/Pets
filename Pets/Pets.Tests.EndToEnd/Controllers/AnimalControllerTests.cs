using FluentAssertions;
using Newtonsoft.Json;
using Pets.Infrastructure.Commands.Animals;
using Pets.Infrastructure.DTO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Pets.Tests.EndToEnd.Controllers
{
    public class AnimalControllerTests : ControllerTestsBase
    {
        private CreateAnimal _existingAnimal;
        private CreateAnimal _existingAnimalForDelete;
        private UpdateAnimal _nonExistingAnimal;

        public AnimalControllerTests()
        {
            // Initializate data on start
            _existingAnimal = new CreateAnimal
            {
                Name = "Tajger",
                YearOfBirth = 2015
            };

            _existingAnimalForDelete = new CreateAnimal
            {
                Name = "AnimalToDelete",
                YearOfBirth = 2017
            };

            _nonExistingAnimal = new UpdateAnimal
            {
                Name = "NonExistingAnimal",
                YearOfBirth = 1920
            };
            
            RegisterAnimalAsync(_existingAnimal);
            RegisterAnimalAsync(_existingAnimalForDelete);
        }

        [Fact]
        public async Task given_valid_animal_name_should_exist()
        {
            //TODO: Pass parameter to GET request.
            var name = _existingAnimal.Name;
            var animal = await GetAnimalAsync(name);

            animal.Name.ShouldBeEquivalentTo(name);
        }

        [Fact]
        public async Task given_existing_user_email_and_animal_name_animal_should_be_created()
        {
            var newAnimal = new CreateAnimal
            {
                Name = "Goldi",
                YearOfBirth = 2004
            };

            //TODO: Why does it return 302 status code instead of 204 ?
            var response = await RegisterAnimalAsync(newAnimal);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NoContent);
        }

        private async Task<AnimalDto> GetAnimalAsync(string name)
        {
            var response = await _client.GetAsync($"users/exist@email.com/animals/{name}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AnimalDto>(responseString);
        }

        private async Task<HttpResponseMessage> RegisterAnimalAsync(CreateAnimal request)
        {
            var payload = GetPayload(request);
            var response = await _client.PostAsync("users/exist@email.com/animals", payload);

            return response;
        }
    }
}
