using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Clients;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ClientTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetClientQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_client()
        {
            // Arrange
            var query = new GetClientQuery { Id = 1 };
            var handler = new GetClientQueryHandler(DbContext);

            var user = new User { Email = "client@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var client = new Client { UserId = user.UserId, PersonalCode = "12345678901", DateOfBirth = DateTime.Now.AddYears(-30), Address = "Test St 1" };
            await DbContext.Clients.AddAsync(client);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
        }

        [Theory]
        [InlineData(101)]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task Get_should_return_null_when_client_does_not_exist(int id)
        {
            // Arrange
            var query = new GetClientQuery { Id = id };
            var handler = new GetClientQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            // Arrange
            var query = (GetClientQuery)null;
            var handler = new GetClientQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}