using System;
using System.Net;
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
    public class AppointmentDocumentsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            var url = "/api/AppointmentDocuments/List?page=1&pageSize=4";
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<AppointmentDocument>>>(url);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_existing_appointment_document()
        {
            var clientUser = new User { Email = "client@test.com", PasswordHash = "hash", FirstName = "Client", LastName = "User", PhoneNumber = "12345678" };
            var doctorUser = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "87654321" };
            await DbContext.Users.AddAsync(clientUser);
            await DbContext.Users.AddAsync(doctorUser);
            await DbContext.SaveChangesAsync();

            var client = new Client { UserId = clientUser.UserId, PersonalCode = "12345678901", DateOfBirth = DateTime.Now.AddYears(-30), Address = "Test St 1" };
            var doctor = new Doctor { UserId = doctorUser.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Clients.AddAsync(client);
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var appointment = new Appointment { ClientId = client.ClientId, DoctorId = doctor.DoctorId, AppointmentDateTime = DateTime.Now };
            await DbContext.Appointments.AddAsync(appointment);
            await DbContext.SaveChangesAsync();

            var document = new AppointmentDocument { AppointmentId = appointment.AppointmentId, DocumentType = "Report", FileName = "test.pdf", FilePath = "/files/test.pdf", FileSize = 1024 };
            await DbContext.AppointmentDocuments.AddAsync(document);
            await DbContext.SaveChangesAsync();

            var url = $"/api/AppointmentDocuments/Get?id={document.DocumentId}";
            var response = await Client.GetFromJsonAsync<OperationResult<AppointmentDocumentDetailsDto>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(document.DocumentId, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_appointment_document()
        {
            var url = "/api/AppointmentDocuments/Get?id=99999";
            var response = await Client.GetAsync(url);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}