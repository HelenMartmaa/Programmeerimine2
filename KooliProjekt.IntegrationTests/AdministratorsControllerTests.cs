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
    public class AdministratorsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            // Arrange
            var url = "/api/Administrators/List?page=1&pageSize=4";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Administrator>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_existing_administrator()
        {
            // Arrange
            var user = new User { Email = "admin@test.com", PasswordHash = "hash", FirstName = "Admin", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var administrator = new Administrator { UserId = user.UserId, Department = "IT" };
            await DbContext.Administrators.AddAsync(administrator);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Administrators/Get?id={administrator.AdminId}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<AdministratorDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(administrator.AdminId, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_administrator()
        {
            // Arrange
            var url = "/api/Administrators/Get?id=99999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

		[Fact]
		public async Task Save_should_create_new_administrator()
		{
			// Arrange
			var user = new User { Email = "admin@test.com", PasswordHash = "hash", FirstName = "Admin", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var command = new { AdminId = 0, UserId = user.UserId, Department = "IT" };

			// Act
			var response = await Client.PostAsJsonAsync("/api/Administrators/Save", command);

			// Assert
			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Save_should_update_existing_administrator()
		{
			// Arrange
			var user = new User { Email = "admin@test.com", PasswordHash = "hash", FirstName = "Admin", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var administrator = new Administrator { UserId = user.UserId, Department = "IT" };
			await DbContext.Administrators.AddAsync(administrator);
			await DbContext.SaveChangesAsync();

			var command = new { AdminId = administrator.AdminId, UserId = user.UserId, Department = "HR" };

			// Act
			var response = await Client.PostAsJsonAsync("/api/Administrators/Save", command);

			// Assert
			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Delete_should_delete_existing_administrator()
		{
			// Arrange
			var user = new User { Email = "admin@test.com", PasswordHash = "hash", FirstName = "Admin", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var administrator = new Administrator { UserId = user.UserId, Department = "IT" };
			await DbContext.Administrators.AddAsync(administrator);
			await DbContext.SaveChangesAsync();

			var command = new { Id = administrator.AdminId };

			// Act
			var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Administrators/Delete")
			{
				Content = JsonContent.Create(command)
			};
			var response = await Client.SendAsync(request);

			// Assert
			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Delete_should_return_not_found_for_missing_administrator()
		{
			// Arrange
			var command = new { Id = 99999 };

			// Act
			var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Administrators/Delete")
			{
				Content = JsonContent.Create(command)
			};
			var response = await Client.SendAsync(request);

			// Assert
			Assert.NotNull(response);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
    }
}