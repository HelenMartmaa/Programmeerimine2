using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Override, to bring User and Appointments
        public override async Task<Client> GetByIdAsync(int id)
        {
            return await DbContext.Clients
                .Include(c => c.User)
                .Include(c => c.Appointments)
                .Where(c => c.ClientId == id)
                .FirstOrDefaultAsync();
        }
    }
}