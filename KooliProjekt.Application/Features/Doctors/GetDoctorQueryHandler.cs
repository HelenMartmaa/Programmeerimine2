using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class GetDoctorQueryHandler : IRequestHandler<GetDoctorQuery, OperationResult<object>>
    {
        private readonly IDoctorRepository _doctorRepository;

        public GetDoctorQueryHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<OperationResult<object>> Handle(GetDoctorQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var doctor = await _doctorRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = doctor.DoctorId,
                UserId = doctor.UserId,
                Specialization = doctor.Specialization,
                DocLicenseNum = doctor.DocLicenseNum,
                WorkingHoursStart = doctor.WorkingHoursStart,
                WorkingHoursEnd = doctor.WorkingHoursEnd,
                User = doctor.User != null ? new
                {
                    Id = doctor.User.UserId,
                    FirstName = doctor.User.FirstName,
                    LastName = doctor.User.LastName,
                    Email = doctor.User.Email
                } : null,
                Appointments = doctor.Appointments?.Select(a => new
                {
                    Id = a.AppointmentId,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status
                })
            };

            return result;
        }
    }
}