using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Override, to bring Client, Doctor, Administrator
        public override async Task<User> GetByIdAsync(int id)
        {
            return await DbContext.Users
                .Include(u => u.Client)
                .Include(u => u.Doctor)
                .Include(u => u.Administrator)
                .Where(u => u.UserId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}