using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class AppointmentDocumentRepository : BaseRepository<AppointmentDocument>, IAppointmentDocumentRepository
    {
        public AppointmentDocumentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Override, to bring Appointment
        public override async Task<AppointmentDocument> GetByIdAsync(int id)
        {
            return await DbContext.AppointmentDocuments
                .Include(ad => ad.Appointment)
                .Where(ad => ad.DocumentId == id)
                .FirstOrDefaultAsync();
        }
    }
}