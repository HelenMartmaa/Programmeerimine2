using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class AdministratorRepository : BaseRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Override, to bring User
        public override async Task<Administrator> GetByIdAsync(int id)
        {
            return await DbContext.Administrators
                .Include(a => a.User)
                .Where(a => a.AdminId == id)
                .FirstOrDefaultAsync();
        }
    }
}