using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IAppointmentDocumentRepository
    {
        Task<AppointmentDocument> GetByIdAsync(int id);
        Task SaveAsync(AppointmentDocument document);
        Task DeleteAsync(AppointmentDocument document);
    }
}