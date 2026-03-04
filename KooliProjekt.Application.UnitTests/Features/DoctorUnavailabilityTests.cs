using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.DoctorUnavailabilities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class DoctorUnavailabilityTests : TestBase
    {
        // GET tests
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetDoctorUnavailabilityQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetDoctorUnavailabilityQuery)null;
            var handler = new GetDoctorUnavailabilityQueryHandler(DbContext);

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
            var query = new GetDoctorUnavailabilityQuery { Id = id };
            var handler = new GetDoctorUnavailabilityQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_doctor_unavailability()
        {
            // Arrange
            var query = new GetDoctorUnavailabilityQuery { Id = 1 };
            var handler = new GetDoctorUnavailabilityQueryHandler(DbContext);

            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Vacation" };
            await DbContext.DoctorUnavailabilities.AddAsync(unavailability);
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
        public async Task Get_should_return_null_when_doctor_unavailability_does_not_exist(int id)
        {
            // Arrange
            var query = new GetDoctorUnavailabilityQuery { Id = id };
            var handler = new GetDoctorUnavailabilityQueryHandler(DbContext);

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
                new ListDoctorUnavailabilitiesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListDoctorUnavailabilitiesQuery)null;
            var handler = new ListDoctorUnavailabilitiesQueryHandler(DbContext);

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
            var query = new ListDoctorUnavailabilitiesQuery { Page = page, PageSize = pageSize };
            var handler = new ListDoctorUnavailabilitiesQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_doctor_unavailabilities()
        {
            // Arrange
            var query = new ListDoctorUnavailabilitiesQuery { Page = 1, PageSize = 5 };
            var handler = new ListDoctorUnavailabilitiesQueryHandler(DbContext);

            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            foreach (var i in Enumerable.Range(1, 15))
            {
                var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now.AddDays(i), EndDate = DateTime.Now.AddDays(i + 1), Reason = $"Reason {i}" };
                await DbContext.DoctorUnavailabilities.AddAsync(unavailability);
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
        public async Task List_should_return_empty_result_if_doctor_unavailabilities_dont_exist()
        {
            // Arrange
            var query = new ListDoctorUnavailabilitiesQuery { Page = 1, PageSize = 5 };
            var handler = new ListDoctorUnavailabilitiesQueryHandler(DbContext);

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
                new DeleteDoctorUnavailabilityCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteDoctorUnavailabilityCommand)null;
            var handler = new DeleteDoctorUnavailabilityCommandHandler(DbContext);

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
            var query = new DeleteDoctorUnavailabilityCommand { Id = id };
            var handler = new DeleteDoctorUnavailabilityCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_doctor_unavailability()
        {
            // Arrange
            var query = new DeleteDoctorUnavailabilityCommand { Id = 1 };
            var handler = new DeleteDoctorUnavailabilityCommandHandler(DbContext);

            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Vacation" };
            await DbContext.DoctorUnavailabilities.AddAsync(unavailability);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.DoctorUnavailabilities.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_doctor_unavailability()
        {
            // Arrange
            var query = new DeleteDoctorUnavailabilityCommand { Id = 1034 };
            var handler = new DeleteDoctorUnavailabilityCommandHandler(DbContext);

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
                new SaveDoctorUnavailabilityCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveDoctorUnavailabilityCommand)null;
            var handler = new SaveDoctorUnavailabilityCommandHandler(DbContext);

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
            var query = new SaveDoctorUnavailabilityCommand { UnavailabilityId = -1 };
            var handler = new SaveDoctorUnavailabilityCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_doctor_unavailability()
        {
            // Arrange
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var query = new SaveDoctorUnavailabilityCommand { UnavailabilityId = 0, DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Vacation" };
            var handler = new SaveDoctorUnavailabilityCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.DoctorUnavailabilities.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(query.Reason, saved.Reason);
        }

        [Fact]
        public async Task Save_should_update_existing_doctor_unavailability()
        {
            // Arrange
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Old Reason" };
            await DbContext.DoctorUnavailabilities.AddAsync(unavailability);
            await DbContext.SaveChangesAsync();

            var query = new SaveDoctorUnavailabilityCommand { UnavailabilityId = 1, DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "New Reason" };
            var handler = new SaveDoctorUnavailabilityCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.DoctorUnavailabilities.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(query.Reason, saved.Reason);
        }

        [Fact]
        public async Task Save_should_return_error_when_doctor_unavailability_does_not_exist()
        {
            // Arrange
            var query = new SaveDoctorUnavailabilityCommand { UnavailabilityId = 999, Reason = "Test" };
            var handler = new SaveDoctorUnavailabilityCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}