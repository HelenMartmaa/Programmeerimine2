using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.InvoiceRows;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class InvoiceRowTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetInvoiceRowQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_invoice_row()
        {
            // Arrange
            var query = new GetInvoiceRowQuery { Id = 1 };
            var handler = new GetInvoiceRowQueryHandler(DbContext);

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

            var invoice = new Invoice { AppointmentId = appointment.AppointmentId, DueDate = DateTime.Now.AddDays(30), InvoiceNum = "INV001" };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var invoiceRow = new InvoiceRow { InvoiceId = invoice.InvoiceId, ServiceDescription = "Test Service", Fee = 50, Quantity = 1, Discount = 0 };
            await DbContext.InvoiceRows.AddAsync(invoiceRow);
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
        public async Task Get_should_return_null_when_invoice_row_does_not_exist(int id)
        {
            // Arrange
            var query = new GetInvoiceRowQuery { Id = id };
            var handler = new GetInvoiceRowQueryHandler(DbContext);

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
            var query = (GetInvoiceRowQuery)null;
            var handler = new GetInvoiceRowQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}