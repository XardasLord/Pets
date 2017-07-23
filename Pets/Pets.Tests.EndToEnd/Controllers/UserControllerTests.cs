using FluentAssertions;
using Newtonsoft.Json;
using Pets.Infrastructure.Commands.Users;
using Pets.Infrastructure.DTO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Pets.Tests.EndToEnd.Controllers
{
    public class UserControllerTests : ControllerTestsBase
    {
        private CreateUser _existingUser;
        private CreateUser _existingUserForDelete;
        private UpdateUser _nonExistingUser;

        public UserControllerTests()
        {
            // Initializate data on start
            _existingUser = new CreateUser
            {
                Email = "exist@email.com",
                FirstName = "exist",
                LastName = "exist",
                Password = "secret"
            };

            _existingUserForDelete = new CreateUser
            {
                Email = "existForDelete@email.com",
                FirstName = "existForDetele",
                LastName = "existForDetele",
                Password = "secret"
            };

            _nonExistingUser = new UpdateUser
            {
                Email = "test1000@email.com",
                FirstName = "Non existing user name",
                LastName = "Non existing user name",
                Password = "secret"
            };

            RegisterUserAsync(_existingUser);
            RegisterUserAsync(_existingUserForDelete);
        }

        [Fact]
        public async Task given_valid_email_user_should_exist()
        {
            var email = _existingUser.Email;
            var user = await GetUserAsync(email);

            user.Email.ShouldBeEquivalentTo(email);
        }

        [Fact]
        public async Task given_invalid_email_user_should_throw_exception()
        {
            var response = await _client.GetAsync($"users/{_nonExistingUser.Email}");

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task given_unique_email_user_should_be_created_and_return_201_status_code()
        {
            var request = new CreateUser
            {
                Email = "unique@email.com",
                FirstName = "test",
                LastName = "test",
                Password = "secret"
            };

            var response = await RegisterUserAsync(request);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.ToString().ShouldBeEquivalentTo($"users/{request.Email}");

            var user = await GetUserAsync(request.Email);
            user.Email.ShouldBeEquivalentTo(request.Email);
        }

        [Fact]
        public async Task given_email_to_register_which_already_exists_should_throw_an_exception()
        {
            var response = await RegisterUserAsync(_existingUser);
            
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task update_user_data_on_existing_email_should_return_NoContent_204_status_code()
        {
            var request = new UpdateUser
            {
                Email = _existingUser.Email,
                FirstName = "Changed firstname",
                LastName = "Changed lastname",
                Password = "NewSecretPassword"
            };
            var payload = GetPayload(request);

            var response = await _client.PutAsync($"users/{_existingUser.Email}", payload);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task update_user_data_on_non_existing_email_should_throw_an_exception()
        {
            var payload = GetPayload(_nonExistingUser);

            var response = await _client.PutAsync($"users/{_nonExistingUser.Email}", payload);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.InternalServerError);
            //await Assert.ThrowsAnyAsync<Exception>(
            //    async () => await _client.PutAsync($"users/{_nonExistingUser.Email}", payload));
        }

        [Fact]
        public async Task delete_user_on_existing_email_should_return_NoContent_204_status_code_and_getting_that_user_should_throw_an_exception()
        {
            var response = await _client.DeleteAsync($"users/{_existingUserForDelete.Email}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NoContent);

            response = await _client.GetAsync($"users/{_existingUserForDelete.Email}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task delete_user_on_non_existing_email_should_throw_an_exception()
        {
            var response = await _client.DeleteAsync($"users/{_nonExistingUser.Email}");

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task login_user_with_correct_data_should_return_302_status_code()
        {
            var request = new LogInUser
            {
                Email = _existingUser.Email,
                Password = _existingUser.Password
            };
            var payload = GetPayload(request);

            var response = await _client.PostAsync($"users/login", payload);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Found);
        }

        [Fact]
        public async Task login_user_with_incorrect_data_should_throw_an_exception()
        {
            var request = new LogInUser
            {
                Email = _existingUser.Email,
                Password = "wrongPassword"
            };
            var payload = GetPayload(request);

            var response = await _client.PostAsync($"users/login", payload);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task logout_user_should_return_200_status_code()
        {
            var response = await _client.GetAsync($"users/logout");

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }

        private async Task<UserDto> GetUserAsync(string email)
        {
            var response = await _client.GetAsync($"users/{email}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UserDto>(responseString);
        }

        private async Task<HttpResponseMessage> RegisterUserAsync(CreateUser request)
        {
            var payload = GetPayload(request);
            var response = await _client.PostAsync("users", payload);

            return response;
        }
    }
}
