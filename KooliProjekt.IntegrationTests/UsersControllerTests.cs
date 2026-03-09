using System;
using System.Net;
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
    }
}