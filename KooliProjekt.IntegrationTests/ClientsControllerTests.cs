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
    public class ClientsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            var url = "/api/Clients/List?page=1&pageSize=4";
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Client>>>(url);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_existing_client()
        {
            var user = new User { Email = "client@test.com", PasswordHash = "hash", FirstName = "Client", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var client = new Client { UserId = user.UserId, PersonalCode = "12345678901", DateOfBirth = DateTime.Now.AddYears(-30), Address = "Test St 1" };
            await DbContext.Clients.AddAsync(client);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Clients/Get?id={client.ClientId}";
            var response = await Client.GetFromJsonAsync<OperationResult<ClientDetailsDto>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(client.ClientId, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_client()
        {
            var url = "/api/Clients/Get?id=99999";
            var response = await Client.GetAsync(url);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

		[Fact]
		public async Task Save_should_create_new_client()
		{
			var user = new User { Email = "client@test.com", PasswordHash = "hash", FirstName = "Client", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var command = new { ClientId = 0, UserId = user.UserId, PersonalCode = "12345678901", DateOfBirth = DateTime.Now.AddYears(-30), Address = "Test St 1" };
			var response = await Client.PostAsJsonAsync("/api/Clients/Save", command);

			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Save_should_update_existing_client()
		{
			var user = new User { Email = "client@test.com", PasswordHash = "hash", FirstName = "Client", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var client = new Client { UserId = user.UserId, PersonalCode = "12345678901", DateOfBirth = DateTime.Now.AddYears(-30), Address = "Test St 1" };
			await DbContext.Clients.AddAsync(client);
			await DbContext.SaveChangesAsync();

			var command = new { ClientId = client.ClientId, UserId = user.UserId, PersonalCode = "12345678901", DateOfBirth = DateTime.Now.AddYears(-25), Address = "New St 2" };
			var response = await Client.PostAsJsonAsync("/api/Clients/Save", command);

			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Delete_should_delete_existing_client()
		{
			var user = new User { Email = "client@test.com", PasswordHash = "hash", FirstName = "Client", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var client = new Client { UserId = user.UserId, PersonalCode = "12345678901", DateOfBirth = DateTime.Now.AddYears(-30), Address = "Test St 1" };
			await DbContext.Clients.AddAsync(client);
			await DbContext.SaveChangesAsync();

			var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Clients/Delete")
			{
				Content = JsonContent.Create(new { Id = client.ClientId })
			};
			var response = await Client.SendAsync(request);

			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Delete_should_return_error_for_missing_client()
		{
			var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Clients/Delete")
			{
				Content = JsonContent.Create(new { Id = 99999 })
			};
			var response = await Client.SendAsync(request);

			Assert.NotNull(response);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
    }
}