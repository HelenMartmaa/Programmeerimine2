using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Users;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class UserTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetUserQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_user()
        {
            // Arrange
            var query = new GetUserQuery { Id = 1 };
            var handler = new GetUserQueryHandler(DbContext);

            var user = new User { Email = "test@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
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
        public async Task Get_should_return_null_when_user_does_not_exist(int id)
        {
            // Arrange
            var query = new GetUserQuery { Id = id };
            var handler = new GetUserQueryHandler(DbContext);

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
            var query = (GetUserQuery)null;
            var handler = new GetUserQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}