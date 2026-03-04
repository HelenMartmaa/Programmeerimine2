using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Doctors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class DoctorTests : TestBase
    {
        // GET tests
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetDoctorQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetDoctorQuery)null;
            var handler = new GetDoctorQueryHandler(DbContext);

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
            var query = new GetDoctorQuery { Id = id };
            var handler = new GetDoctorQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_doctor()
        {
            // Arrange
            var query = new GetDoctorQuery { Id = 1 };
            var handler = new GetDoctorQueryHandler(DbContext);

            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
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
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_doctor_does_not_exist(int id)
        {
            // Arrange
            var query = new GetDoctorQuery { Id = id };
            var handler = new GetDoctorQueryHandler(DbContext);

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
                new ListDoctorsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListDoctorsQuery)null;
            var handler = new ListDoctorsQueryHandler(DbContext);

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
            var query = new ListDoctorsQuery { Page = page, PageSize = pageSize };
            var handler = new ListDoctorsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_doctors()
        {
            // Arrange
            var query = new ListDoctorsQuery { Page = 1, PageSize = 5 };
            var handler = new ListDoctorsQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                var user = new User { Email = $"doctor{i}@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = $"User{i}", PhoneNumber = "12345678" };
                await DbContext.Users.AddAsync(user);
                await DbContext.SaveChangesAsync();

                var doctor = new Doctor { UserId = user.UserId, Specialization = $"Specialization {i}", DocLicenseNum = $"LIC00{i}" };
                await DbContext.Doctors.AddAsync(doctor);
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
        public async Task List_should_return_empty_result_if_doctors_dont_exist()
        {
            // Arrange
            var query = new ListDoctorsQuery { Page = 1, PageSize = 5 };
            var handler = new ListDoctorsQueryHandler(DbContext);

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
                new DeleteDoctorCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteDoctorCommand)null;
            var handler = new DeleteDoctorCommandHandler(DbContext);

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
            var query = new DeleteDoctorCommand { Id = id };
            var handler = new DeleteDoctorCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_doctor()
        {
            // Arrange
            var query = new DeleteDoctorCommand { Id = 1 };
            var handler = new DeleteDoctorCommandHandler(DbContext);

            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Doctors.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_doctor()
        {
            // Arrange
            var query = new DeleteDoctorCommand { Id = 1034 };
            var handler = new DeleteDoctorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

		[Fact]
		public async Task Delete_should_delete_doctor_with_appointments_and_unavailabilities()
		{
			// Arrange
			var query = new DeleteDoctorCommand { Id = 1 };
			var handler = new DeleteDoctorCommandHandler(DbContext);

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

			var invoice = new Invoice { AppointmentId = appointment.AppointmentId, InvoiceDate = DateTime.Now, DueDate = DateTime.Now.AddDays(30), InvoiceNum = "INV-001" };
			await DbContext.Invoices.AddAsync(invoice);
			await DbContext.SaveChangesAsync();

			var invoiceRow = new InvoiceRow { InvoiceId = invoice.InvoiceId, ServiceDescription = "Consultation", Fee = 50.00m, Quantity = 1, Discount = 0 };
			await DbContext.InvoiceRows.AddAsync(invoiceRow);

			var unavailability = new DoctorUnavailability { DoctorId = doctor.DoctorId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), Reason = "Vacation" };
			await DbContext.DoctorUnavailabilities.AddAsync(unavailability);
			await DbContext.SaveChangesAsync();

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.False(result.HasErrors);
			Assert.Equal(0, DbContext.Doctors.Count());
			Assert.Equal(0, DbContext.Appointments.Count());
			Assert.Equal(0, DbContext.AppointmentDocuments.Count());
			Assert.Equal(0, DbContext.Invoices.Count());
			Assert.Equal(0, DbContext.InvoiceRows.Count());
			Assert.Equal(0, DbContext.DoctorUnavailabilities.Count());
		}

        // SAVE tests
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveDoctorCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveDoctorCommand)null;
            var handler = new SaveDoctorCommandHandler(DbContext);

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
            var query = new SaveDoctorCommand { DoctorId = -1 };
            var handler = new SaveDoctorCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_doctor()
        {
            // Arrange
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var query = new SaveDoctorCommand { DoctorId = 0, UserId = user.UserId, Specialization = "General", DocLicenseNum = "LIC001" };
            var handler = new SaveDoctorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.Doctors.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(query.Specialization, saved.Specialization);
        }

        [Fact]
        public async Task Save_should_update_existing_doctor()
        {
            // Arrange
            var user = new User { Email = "doctor@test.com", PasswordHash = "hash", FirstName = "Doctor", LastName = "User", PhoneNumber = "12345678" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var doctor = new Doctor { UserId = user.UserId, Specialization = "Old Specialization", DocLicenseNum = "LIC001" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var query = new SaveDoctorCommand { DoctorId = 1, UserId = user.UserId, Specialization = "New Specialization", DocLicenseNum = "LIC001" };
            var handler = new SaveDoctorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.Doctors.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(query.Specialization, saved.Specialization);
        }

        [Fact]
        public async Task Save_should_return_error_when_doctor_does_not_exist()
        {
            // Arrange
            var query = new SaveDoctorCommand { DoctorId = 999, Specialization = "General" };
            var handler = new SaveDoctorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}