using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class UserTests : TestBase
    {
        // GET tests
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetUserQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetUserQuery)null;
            var handler = new GetUserQueryHandler(DbContext);

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
            var query = new GetUserQuery { Id = id };
            var handler = new GetUserQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_user()
        {
            // Arrange
            var query = new GetUserQuery { Id = 1 };
            var handler = new GetUserQueryHandler(DbContext);

            var user = new User { Email = "user@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
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

        // LIST tests
        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListUsersQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListUsersQuery)null;
            var handler = new ListUsersQueryHandler(DbContext);

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
            var query = new ListUsersQuery { Page = page, PageSize = pageSize };
            var handler = new ListUsersQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_users()
        {
            // Arrange
            var query = new ListUsersQuery { Page = 1, PageSize = 5 };
            var handler = new ListUsersQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                var user = new User { Email = $"user{i}@test.com", PasswordHash = "hash", FirstName = "Test", LastName = $"User{i}", PhoneNumber = "12345678" };
                await DbContext.Users.AddAsync(user);
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
        public async Task List_should_return_empty_result_if_users_dont_exist()
        {
            // Arrange
            var query = new ListUsersQuery { Page = 1, PageSize = 5 };
            var handler = new ListUsersQueryHandler(DbContext);

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
                new DeleteUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteUserCommand)null;
            var handler = new DeleteUserCommandHandler(DbContext);

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
            var query = new DeleteUserCommand { Id = id };
            var handler = new DeleteUserCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_user()
        {
            // Arrange
            var query = new DeleteUserCommand { Id = 1 };
            var handler = new DeleteUserCommandHandler(DbContext);

            var user = new User { Email = "user@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Users.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_user()
        {
            // Arrange
            var query = new DeleteUserCommand { Id = 1034 };
            var handler = new DeleteUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_user_with_related_roles()
        {
            // Arrange
            var query = new DeleteUserCommand { Id = 1 };
            var handler = new DeleteUserCommandHandler(DbContext);

            var user = new User { Email = "user@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
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
            Assert.Equal(0, DbContext.Users.Count());
            Assert.Equal(0, DbContext.Administrators.Count());
        }

		[Fact]
		public async Task Delete_should_delete_user_with_client_role()
		{
			// Arrange
			var query = new DeleteUserCommand { Id = 1 };
			var handler = new DeleteUserCommandHandler(DbContext);

			var user = new User { Email = "user@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
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
			Assert.Equal(0, DbContext.Users.Count());
			Assert.Equal(0, DbContext.Clients.Count());
		}

		[Fact]
		public async Task Delete_should_delete_user_with_doctor_role()
		{
			// Arrange
			var query = new DeleteUserCommand { Id = 1 };
			var handler = new DeleteUserCommandHandler(DbContext);

			var user = new User { Email = "user@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
			await DbContext.Doctors.AddAsync(doctor);
			await DbContext.SaveChangesAsync();

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.False(result.HasErrors);
			Assert.Equal(0, DbContext.Users.Count());
			Assert.Equal(0, DbContext.Doctors.Count());
		}

        // SAVE tests
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveUserCommand)null;
            var handler = new SaveUserCommandHandler(DbContext);

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
            var query = new SaveUserCommand { UserId = -1 };
            var handler = new SaveUserCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_user()
        {
            // Arrange
            var query = new SaveUserCommand { UserId = 0, Email = "new@test.com", PasswordHash = "hash", FirstName = "New", LastName = "User", PhoneNumber = "12345678" };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.Users.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(query.Email, saved.Email);
        }

        [Fact]
        public async Task Save_should_update_existing_user()
        {
            // Arrange
            var user = new User { Email = "old@test.com", PasswordHash = "hash", FirstName = "Old", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var query = new SaveUserCommand { UserId = 1, Email = "new@test.com", PasswordHash = "hash", FirstName = "New", LastName = "User", PhoneNumber = "12345678" };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.Users.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(query.Email, saved.Email);
        }

        [Fact]
        public async Task Save_should_return_error_when_user_does_not_exist()
        {
            // Arrange
            var query = new SaveUserCommand { UserId = 999, Email = "test@test.com" };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}