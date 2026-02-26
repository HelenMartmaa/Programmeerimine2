using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Administrators;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class AdministratorTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetAdministratorQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_administrator()
        {
            // Arrange
            var query = new GetAdministratorQuery { Id = 1 };
            var handler = new GetAdministratorQueryHandler(DbContext);

            var user = new User { Email = "test@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var administrator = new Administrator { UserId = user.UserId, Department = "Test Department" };
            await DbContext.Administrators.AddAsync(administrator);
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
        public async Task Get_should_return_null_when_administrator_does_not_exist(int id)
        {
            // Arrange
            var query = new GetAdministratorQuery { Id = id };
            var handler = new GetAdministratorQueryHandler(DbContext);

            var user = new User { Email = "test@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var administrator = new Administrator { UserId = user.UserId, Department = "Test Department" };
            await DbContext.Administrators.AddAsync(administrator);
            await DbContext.SaveChangesAsync();

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
            var query = (GetAdministratorQuery)null;
            var handler = new GetAdministratorQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}