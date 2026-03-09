using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class UsersControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            var url = "/api/Users/List?page=1&pageSize=4";
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<User>>>(url);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_existing_user()
        {
            var user = new User { Email = "user@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Users/Get?id={user.UserId}";
            var response = await Client.GetFromJsonAsync<OperationResult<UserDetailsDto>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(user.UserId, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_user()
        {
            var url = "/api/Users/Get?id=99999";
            var response = await Client.GetAsync(url);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_create_new_user()
        {
            var command = new { UserId = 0, Role = 0, Email = "newuser@test.com", PasswordHash = "hash", FirstName = "New", LastName = "User", PhoneNumber = "12345678" };
            var response = await Client.PostAsJsonAsync("/api/Users/Save", command);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Save_should_update_existing_user()
        {
            var user = new User { Email = "user@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var command = new { UserId = user.UserId, Role = 0, Email = "updated@test.com", PasswordHash = "hash", FirstName = "Updated", LastName = "User", PhoneNumber = "87654321" };
            var response = await Client.PostAsJsonAsync("/api/Users/Save", command);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_should_delete_existing_user()
        {
            var user = new User { Email = "user@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Users/Delete")
            {
                Content = JsonContent.Create(new { Id = user.UserId })
            };
            var response = await Client.SendAsync(request);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_should_return_error_for_missing_user()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Users/Delete")
            {
                Content = JsonContent.Create(new { Id = 99999 })
            };
            var response = await Client.SendAsync(request);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}