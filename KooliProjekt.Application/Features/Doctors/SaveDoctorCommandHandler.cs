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
    public class SaveDoctorCommandHandler : IRequestHandler<SaveDoctorCommand, OperationResult>
    {
        private readonly IDoctorRepository _doctorRepository;

        public SaveDoctorCommandHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<OperationResult> Handle(SaveDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = new Doctor();
            
            if (request.DoctorId != 0)
            {
                doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
            }

            doctor.UserId = request.UserId;
            doctor.Specialization = request.Specialization;
            doctor.DocLicenseNum = request.DocLicenseNum;
            doctor.WorkingHoursStart = request.WorkingHoursStart;
            doctor.WorkingHoursEnd = request.WorkingHoursEnd;

            await _doctorRepository.SaveAsync(doctor);

            return new OperationResult();
        }
    }
}