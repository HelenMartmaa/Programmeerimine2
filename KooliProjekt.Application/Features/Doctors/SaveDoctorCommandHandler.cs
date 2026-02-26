using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class SaveDoctorCommandHandler : IRequestHandler<SaveDoctorCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveDoctorCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveDoctorCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.DoctorId < 0)
            {
                return result.AddPropertyError(nameof(request.DoctorId), "Id cannot be negative.");
            }

            var doctor = new Doctor();
            if (request.DoctorId == 0)
            {
                await _dbContext.Doctors.AddAsync(doctor);
            }
            else
            {
                doctor = await _dbContext.Doctors.FindAsync(request.DoctorId);
                if (doctor == null)
                {
                    return result.AddPropertyError(nameof(request.DoctorId), "Doctor with the specified Id does not exist.");
                }
            }

            doctor.UserId = request.UserId;
            doctor.Specialization = request.Specialization;
            doctor.DocLicenseNum = request.DocLicenseNum;
            doctor.WorkingHoursStart = request.WorkingHoursStart;
            doctor.WorkingHoursEnd = request.WorkingHoursEnd;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}