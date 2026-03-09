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
    public class InvoiceRowsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            var url = "/api/InvoiceRows/List?page=1&pageSize=4";
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<InvoiceRow>>>(url);
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_existing_invoice_row()
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

            var invoice = new Invoice { AppointmentId = appointment.AppointmentId, InvoiceDate = DateTime.Now, DueDate = DateTime.Now.AddDays(30), InvoiceNum = "INV-001" };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var invoiceRow = new InvoiceRow { InvoiceId = invoice.InvoiceId, ServiceDescription = "Consultation", Fee = 50.00m, Quantity = 1, Discount = 0 };
            await DbContext.InvoiceRows.AddAsync(invoiceRow);
            await DbContext.SaveChangesAsync();

            var url = $"/api/InvoiceRows/Get?id={invoiceRow.InvoiceRowId}";
            var response = await Client.GetFromJsonAsync<OperationResult<InvoiceRowDetailsDto>>(url);

            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(invoiceRow.InvoiceRowId, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_invoice_row()
        {
            var url = "/api/InvoiceRows/Get?id=99999";
            var response = await Client.GetAsync(url);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}