using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.DoctorUnavailabilities;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class DoctorUnavailabilityTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetDoctorUnavailabilityQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_doctor_unavailability()
        {
            // Arrange
            var query = new GetDoctorUnavailabilityQuery { Id = 1 };
            var handler = new GetDoctorUnavailabilityQueryHandler(DbContext);

            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Test", LastName = "Doctor", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Reason = "Vacation" };
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
        [InlineData(0)]
        [InlineData(-5)]
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

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            // Arrange
            var query = (GetDoctorUnavailabilityQuery)null;
            var handler = new GetDoctorUnavailabilityQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}