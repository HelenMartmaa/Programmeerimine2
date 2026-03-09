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
    public class DoctorUnavailabilitiesControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            var url = "/api/DoctorUnavailabilities/List?page=1&pageSize=4";
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<DoctorUnavailability>>>(url);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_existing_doctor_unavailability()
        {
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Vacation" };
            await DbContext.DoctorUnavailabilities.AddAsync(unavailability);
            await DbContext.SaveChangesAsync();

            var url = $"/api/DoctorUnavailabilities/Get?id={unavailability.UnavailabilityId}";
            var response = await Client.GetFromJsonAsync<OperationResult<DoctorUnavailabilityDetailsDto>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(unavailability.UnavailabilityId, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_doctor_unavailability()
        {
            var url = "/api/DoctorUnavailabilities/Get?id=99999";
            var response = await Client.GetAsync(url);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_create_new_doctor_unavailability()
        {
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var command = new { UnavailabilityId = 0, DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Vacation" };
            var response = await Client.PostAsJsonAsync("/api/DoctorUnavailabilities/Save", command);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Save_should_update_existing_doctor_unavailability()
        {
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Vacation" };
            await DbContext.DoctorUnavailabilities.AddAsync(unavailability);
            await DbContext.SaveChangesAsync();

            var command = new { UnavailabilityId = unavailability.UnavailabilityId, DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Reason = "Sick leave" };
            var response = await Client.PostAsJsonAsync("/api/DoctorUnavailabilities/Save", command);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_should_delete_existing_doctor_unavailability()
        {
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Vacation" };
            await DbContext.DoctorUnavailabilities.AddAsync(unavailability);
            await DbContext.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/DoctorUnavailabilities/Delete")
            {
                Content = JsonContent.Create(new { Id = unavailability.UnavailabilityId })
            };
            var response = await Client.SendAsync(request);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_should_return_error_for_missing_doctor_unavailability()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/DoctorUnavailabilities/Delete")
            {
                Content = JsonContent.Create(new { Id = 99999 })
            };
            var response = await Client.SendAsync(request);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}