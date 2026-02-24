using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class InvoiceRowRepository : BaseRepository<InvoiceRow>, IInvoiceRowRepository
    {
        public InvoiceRowRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Override, to bring Invoice
        public override async Task<InvoiceRow> GetByIdAsync(int id)
        {
            return await DbContext.InvoiceRows
                .Include(ir => ir.Invoice)
                .Where(ir => ir.InvoiceRowId == id)
                .FirstOrDefaultAsync();
        }
    }
}