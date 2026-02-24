using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IInvoiceRowRepository
    {
        Task<InvoiceRow> GetByIdAsync(int id);
        Task SaveAsync(InvoiceRow invoiceRow);
        Task DeleteAsync(InvoiceRow invoiceRow);
    }
}