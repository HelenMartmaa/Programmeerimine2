using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    // Based on 15.11.2025 class 
    // SeedData class to generate data
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IList<Administrator> _administrators = new List<Administrator>();
        private readonly IList<Appointment> _appointments = new List<Appointment>();
        private readonly IList<AppointmentDocument> _appointmentDocuments = new List<AppointmentDocument>();
        private readonly IList<Doctor> _doctors = new List<Doctor>();
        private readonly IList<DoctorUnavailability> _doctorUnavailabilities = new List<DoctorUnavailability>();
        private readonly IList<Invoice> _invoices = new List<Invoice>();
        private readonly IList<InvoiceRow> _invoiceRows = new List<InvoiceRow>();
        private readonly IList<User> _users = new List<User>();
        private readonly IList<Client> _clients = new List<Client>();

        public SeedData(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        // Generate methods
        public void Generate()
        {
            // Not running the functions if data is already existing
            if (_dbContext.Users.Any())
            {
                return;
            }
            // Order here matters!
            GenerateUsers();                    // Users
            GenerateClients();                  // Clients (needs Users)
            GenerateDoctors();                  // Doctors (needs Users)
            GenerateAdministrators();           // Administrators (needs Users)
            GenerateDoctorUnavailabilities();   // DoctorUnavailabilities (needs Doctors)
            GenerateAppointments();             // Appointments (needs Clients and Doctors)
            GenerateInvoices();                 // Invoices (needs Appointments) - includes generation of InvoiceRows!
            GenerateAppointmentDocuments();     // AppointmentDocuments (needs Appointments)

            _dbContext.SaveChanges();
        }

        private void GenerateUsers()
        {
            for (var i = 0; i < 60; i++)
            {
                UserRole role;
                if (i <= 35)
                    role = UserRole.Client;
                else if (i <= 50)
                    role = UserRole.Doctor;
                else
                    role = UserRole.Administrator; // 9 admins

                var user = new User
                {
                    Role = role,
                    Email = $"user{i}@clinic.com",
                    PasswordHash = $"hashed_password_{i}",
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    PhoneNumber = $"+372555{i:D4}",
                    CreatedAt = DateTime.Now.AddDays(-i)
                };

                _users.Add(user);
            }

            _dbContext.Users.AddRange(_users);
            _dbContext.SaveChanges(); // Save in order to get UserId-s
        }

        private void GenerateClients()
        {
            var clientUsers = _users.Where(u => u.Role == UserRole.Client).ToList();

            foreach (var user in clientUsers)
            {
                var client = new Client
                {
                    UserId = user.UserId,
                    PersonalCode = $"3{user.UserId:D10}", //Currently all males...
                    DateOfBirth = DateTime.Now.AddYears(-25 - (user.UserId % 40)),
                    Address = $"Tallinn, Liivalaia {user.UserId}"
                };

                _clients.Add(client);
            }

            _dbContext.Clients.AddRange(_clients);
            _dbContext.SaveChanges();
        }

        private void GenerateDoctors()
        {
            var doctorUsers = _users.Where(u => u.Role == UserRole.Doctor).ToList();
            var specializations = new[] { "Perearst", "Hambaarst", "Kardioloog", "Dermatoloog", "Ortopeed", "Pediaater", "Neuroloog", "Laborispetsialist" };

            foreach (var user in doctorUsers)
            {
                var doctor = new Doctor
                {
                    UserId = user.UserId,
                    Specialization = specializations[user.UserId % specializations.Length],
                    DocLicenseNum = $"DOC-{user.UserId:D5}",
                    WorkingHoursStart = new TimeSpan(8, 0, 0),
                    WorkingHoursEnd = new TimeSpan(16, 0, 0)
                };

                _doctors.Add(doctor);
            }

            _dbContext.Doctors.AddRange(_doctors);
            _dbContext.SaveChanges();
        }

        private void GenerateAdministrators()
        {
            var adminUsers = _users.Where(u => u.Role == UserRole.Administrator).ToList();
            var departments = new[] { "Üldregistratuur", "Sisehaiguste_osakond", "Eriarstide_osakond", "Analüüside osakond" };

            foreach (var user in adminUsers)
            {
                var admin = new Administrator
                {
                    UserId = user.UserId,
                    Department = departments[user.UserId % departments.Length]
                };

                _administrators.Add(admin);
            }

            _dbContext.Administrators.AddRange(_administrators);
            _dbContext.SaveChanges();
        }

        private void GenerateAppointments()
        {
            var random = new Random();

            // Add 100 appointments
            for (var i = 1; i <= 100; i++)
            {
                // Random client
                var client = _clients[random.Next(_clients.Count)];

                // Random doctor
                var doctor = _doctors[random.Next(_doctors.Count)];

                // Generate date in the range of +- 30 days
                var daysOffset = random.Next(-30, 30);
                var appointmentDate = DateTime.Now.AddDays(daysOffset);
                appointmentDate = new DateTime(
                    appointmentDate.Year,
                    appointmentDate.Month,
                    appointmentDate.Day,
                    8 + random.Next(8), // 8:00 - 16:00
                    random.Next(2) * 30, //the appointments take place every full and half hour
                    0
                );

                var appointment = new Appointment
                {
                    ClientId = client.ClientId,
                    DoctorId = doctor.DoctorId,
                    AppointmentDateTime = appointmentDate,
                    DurationMinutes = 30, // default length of 1 appointment
                    Status = (AppointmentStatus)(i % 4), // Scheduled, Completed, Cancelled, NoShow
                    IsOutsideWorkingHours = false,
                    Notes = $"Test broneering nr {i}",
                    CreatedAt = DateTime.Now.AddDays(-i),
                    CancelledAt = (i % 4 == 2) ? (DateTime?)DateTime.Now.AddDays(-i + 1) : null
                };

                _dbContext.Appointments.Add(appointment);
            }

            _dbContext.SaveChanges();
        }

        private void GenerateInvoices()
        {
            // Invoices can only be added to completed appointments
            var completedAppointments = _dbContext.Appointments
                .Where(a => a.Status == AppointmentStatus.Completed)
                .ToList();

            var services = new[]
            {
                new { Description = "Konsultatsioon", Fee = 50m },
                new { Description = "Üldine tervisekontroll", Fee = 80m },
                new { Description = "Raviplaani koostamine", Fee = 100m },
                new { Description = "Vereanalüüs", Fee = 30m },
                new { Description = "Vaktsineerimine", Fee = 25m }
            };

            foreach (var appointment in completedAppointments)
            {
                var invoiceDate = appointment.AppointmentDateTime.AddDays(1);
                var dueDate = invoiceDate.AddDays(14);

                // Random service pick
                var service = services[appointment.AppointmentId % services.Length];
                var serviceDesc = service.Description;
                var serviceFee = service.Fee;

                var totalBeforeVat = serviceFee;
                var totalWithVat = totalBeforeVat * 1.22m;

                var invoice = new Invoice
                {
                    AppointmentId = appointment.AppointmentId,
                    InvoiceDate = invoiceDate,
                    DueDate = dueDate,
                    TotalBeforeVat = totalBeforeVat,
                    TotalWithVat = totalWithVat,
                    IsPaid = appointment.AppointmentId % 3 == 0,
                    PaidAt = (appointment.AppointmentId % 3 == 0) ? (DateTime?)invoiceDate.AddDays(7) : null,
                    InvoiceNum = $"INV-2024-{appointment.AppointmentId:D6}"
                };

                _dbContext.Invoices.Add(invoice);
                _dbContext.SaveChanges();

                // Add Invoice rows, this method is nested inside generating invoice method
                var invoiceRow = new InvoiceRow
                {
                    InvoiceId = invoice.InvoiceId,
                    ServiceDescription = serviceDesc,
                    Fee = serviceFee,
                    Quantity = 1,
                    Discount = 0
                };

                _dbContext.InvoiceRows.Add(invoiceRow);
            }

            _dbContext.SaveChanges();
        }

        private void GenerateDoctorUnavailabilities()
        {
            var random = new Random();

            // Add 1-2 unavailability periods to every doctor
            foreach (var doctor in _doctors)
            {
                var unavailabilityCount = random.Next(1, 3); // 1 or 2

                for (var i = 0; i < unavailabilityCount; i++)
                {
                    var startDate = DateTime.Now.AddDays(random.Next(1, 60)); // 60 days into future as cap
                    var endDate = startDate.AddDays(random.Next(1, 10)); // length of unavailability between 1 to 10 days

                    var unavailability = new DoctorUnavailability
                    {
                        DoctorId = doctor.DoctorId,
                        StartDate = startDate,
                        EndDate = endDate,
                        Reason = random.Next(3) switch
                        {
                            0 => "Puhkus",
                            1 => "Koolitus",
                            _ => "Haigus"
                        }
                    };

                    _doctorUnavailabilities.Add(unavailability);
                }
            }

            _dbContext.DoctorUnavailabilities.AddRange(_doctorUnavailabilities);
            _dbContext.SaveChanges();
        }

        private void GenerateAppointmentDocuments()
        {
            var random = new Random();
            var documentTypes = new[] { "Vaktsineerimisinfo", "Analüüsi tulemused", "Tervisetõend", "Retsept", "Edasi suunamine", "Lisa", "Raviplaan" };

            // Add documents to only completed appointments, to half of them
            var completedAppointments = _dbContext.Appointments
                .Where(a => a.Status == AppointmentStatus.Completed)
                .ToList();

            foreach (var appointment in completedAppointments)
            {
                // 50% chanche to add document, documents was not required
                if (random.Next(2) == 0)
                {
                    var docType = documentTypes[random.Next(documentTypes.Length)];

                    var document = new AppointmentDocument
                    {
                        AppointmentId = appointment.AppointmentId,
                        DocumentType = docType,
                        FileName = $"{docType.Replace(" ", "_")}_{appointment.AppointmentId}.pdf",
                        FilePath = $"/documents/appointments/{appointment.AppointmentId}/{docType.Replace(" ", "_")}.pdf",
                        UploadedAt = appointment.AppointmentDateTime.AddHours(2),
                        FileSize = random.Next(10000, 5000000) // limit 10KB - 5MB
                    };

                    _appointmentDocuments.Add(document);
                }
            }

            _dbContext.AppointmentDocuments.AddRange(_appointmentDocuments);
            _dbContext.SaveChanges();
        }
    }
}