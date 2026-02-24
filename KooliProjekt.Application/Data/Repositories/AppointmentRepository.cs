using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Override, to bring Client, Doctor, Invoice, Documents
        public override async Task<Appointment> GetByIdAsync(int id)
        {
            return await DbContext.Appointments
                .Include(a => a.Client)
                .Include(a => a.Doctor)
                .Include(a => a.Invoice)
                .Include(a => a.Documents)
                .Where(a => a.AppointmentId == id)
                .FirstOrDefaultAsync();
        }
    }
}