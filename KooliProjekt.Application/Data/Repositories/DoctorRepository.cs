using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Override, to bring User, Appointments, Unavailabilities
        public override async Task<Doctor> GetByIdAsync(int id)
        {
            return await DbContext.Doctors
                .Include(d => d.User)
                .Include(d => d.Appointments)
                .Include(d => d.Unavailabilities)
                .Where(d => d.DoctorId == id)
                .FirstOrDefaultAsync();
        }
    }
}