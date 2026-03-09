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
    public class DoctorsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            var url = "/api/Doctors/List?page=1&pageSize=4";
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Doctor>>>(url);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_existing_doctor()
        {
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Doctors/Get?id={doctor.DoctorId}";
            var response = await Client.GetFromJsonAsync<OperationResult<DoctorDetailsDto>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(doctor.DoctorId, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_doctor()
        {
            var url = "/api/Doctors/Get?id=99999";
            var response = await Client.GetAsync(url);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

		[Fact]
		public async Task Save_should_create_new_doctor()
		{
			var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var command = new { DoctorId = 0, UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001", WorkingHoursStart = "08:00:00", WorkingHoursEnd = "16:00:00" };
			var response = await Client.PostAsJsonAsync("/api/Doctors/Save", command);

			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Save_should_update_existing_doctor()
		{
			var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
			await DbContext.Doctors.AddAsync(doctor);
			await DbContext.SaveChangesAsync();

			var command = new { DoctorId = doctor.DoctorId, UserId = user.UserId, Specialization = "Cardiology", DocLicenseNum = "LIC002", WorkingHoursStart = "09:00:00", WorkingHoursEnd = "17:00:00" };
			var response = await Client.PostAsJsonAsync("/api/Doctors/Save", command);

			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Delete_should_delete_existing_doctor()
		{
			var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
			await DbContext.Users.AddAsync(user);
			await DbContext.SaveChangesAsync();

			var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
			await DbContext.Doctors.AddAsync(doctor);
			await DbContext.SaveChangesAsync();

			var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Doctors/Delete")
			{
				Content = JsonContent.Create(new { Id = doctor.DoctorId })
			};
			var response = await Client.SendAsync(request);

			Assert.NotNull(response);
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task Delete_should_return_error_for_missing_doctor()
		{
			var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Doctors/Delete")
			{
				Content = JsonContent.Create(new { Id = 99999 })
			};
			var response = await Client.SendAsync(request);

			Assert.NotNull(response);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
    }
}