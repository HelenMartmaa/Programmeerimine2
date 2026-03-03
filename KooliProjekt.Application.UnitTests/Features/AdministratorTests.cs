using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Administrators;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class AdministratorTests : TestBase
    {
        // GET tests
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetAdministratorQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetAdministratorQuery)null;
            var handler = new GetAdministratorQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_request_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetAdministratorQuery { Id = id };
            var handler = new GetAdministratorQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
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

        // LIST tests
        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListAdministratorsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListAdministratorsQuery)null;
            var handler = new ListAdministratorsQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        [InlineData(4, -10)]
        [InlineData(5, -5)]
        [InlineData(0, 0)]
        [InlineData(-5, -10)]
        public async Task List_should_return_null_when_page_or_page_size_is_zero_or_negative(int page, int pageSize)
        {
            // Arrange
            var query = new ListAdministratorsQuery { Page = page, PageSize = pageSize };
            var handler = new ListAdministratorsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_administrators()
        {
            // Arrange
            var query = new ListAdministratorsQuery { Page = 1, PageSize = 5 };
            var handler = new ListAdministratorsQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                var user = new User { Email = $"admin{i}@test.com", PasswordHash = "hash", FirstName = "Test", LastName = $"User{i}", PhoneNumber = "12345678" };
                await DbContext.Users.AddAsync(user);
                await DbContext.SaveChangesAsync();

                var administrator = new Administrator { UserId = user.UserId, Department = $"Department {i}" };
                await DbContext.Administrators.AddAsync(administrator);
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_administrators_dont_exist()
        {
            // Arrange
            var query = new ListAdministratorsQuery { Page = 1, PageSize = 5 };
            var handler = new ListAdministratorsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        // DELETE tests
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteAdministratorCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteAdministratorCommand)null;
            var handler = new DeleteAdministratorCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Delete_should_not_use_dbcontext_if_id_is_zero_or_less(int id)
        {
            // Arrange
            var query = new DeleteAdministratorCommand { Id = id };
            var handler = new DeleteAdministratorCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_administrator()
        {
            // Arrange
            var query = new DeleteAdministratorCommand { Id = 1 };
            var handler = new DeleteAdministratorCommandHandler(DbContext);

            var user = new User { Email = "test@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var administrator = new Administrator { UserId = user.UserId, Department = "Test Department" };
            await DbContext.Administrators.AddAsync(administrator);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Administrators.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_administrator()
        {
            // Arrange
            var query = new DeleteAdministratorCommand { Id = 1034 };
            var handler = new DeleteAdministratorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        // SAVE tests
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveAdministratorCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveAdministratorCommand)null;
            var handler = new SaveAdministratorCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_not_use_dbcontext_if_id_is_negative()
        {
            // Arrange
            var query = new SaveAdministratorCommand { AdminId = -1 };
            var handler = new SaveAdministratorCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_administrator()
        {
            // Arrange
            var user = new User { Email = "test@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var query = new SaveAdministratorCommand { AdminId = 0, UserId = user.UserId, Department = "Test Department" };
            var handler = new SaveAdministratorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.Administrators.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(query.Department, saved.Department);
        }

        [Fact]
        public async Task Save_should_update_existing_administrator()
        {
            // Arrange
            var user = new User { Email = "test@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var administrator = new Administrator { UserId = user.UserId, Department = "Old Department" };
            await DbContext.Administrators.AddAsync(administrator);
            await DbContext.SaveChangesAsync();

            var query = new SaveAdministratorCommand { AdminId = 1, UserId = user.UserId, Department = "New Department" };
            var handler = new SaveAdministratorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.Administrators.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(query.Department, saved.Department);
        }

        [Fact]
        public async Task Save_should_return_error_when_administrator_does_not_exist()
        {
            // Arrange
            var query = new SaveAdministratorCommand { AdminId = 999, Department = "Test Department" };
            var handler = new SaveAdministratorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.Administrators.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.Null(saved);
        }
    }
}