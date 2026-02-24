using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IDoctorUnavailabilityRepository
    {
        Task<DoctorUnavailability> GetByIdAsync(int id);
        Task SaveAsync(DoctorUnavailability unavailability);
        Task DeleteAsync(DoctorUnavailability unavailability);
    }
}