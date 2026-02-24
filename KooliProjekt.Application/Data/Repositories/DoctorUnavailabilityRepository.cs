using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class DoctorUnavailabilityRepository : BaseRepository<DoctorUnavailability>, IDoctorUnavailabilityRepository
    {
        public DoctorUnavailabilityRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Override, to bring Doctor
        public override async Task<DoctorUnavailability> GetByIdAsync(int id)
        {
            return await DbContext.DoctorUnavailabilities
                .Include(du => du.Doctor)
                .Where(du => du.UnavailabilityId == id)
                .FirstOrDefaultAsync();
        }
    }
}