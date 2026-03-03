using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.Administrators
{
  // Based on 15.11.2025 class
  // Handler for deletion command
  public class DeleteAdministratorCommandHandler : IRequestHandler<DeleteAdministratorCommand, OperationResult>
  {
    private readonly ApplicationDbContext _dbContext;

    public DeleteAdministratorCommandHandler(ApplicationDbContext dbContext)
    {
      if (dbContext == null)
      {
        throw new ArgumentNullException(nameof(dbContext));
      }
      _dbContext = dbContext;
    }

    public async Task<OperationResult> Handle(DeleteAdministratorCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
      {
        throw new ArgumentNullException(nameof(request));
      }

      var result = new OperationResult();

      if (request.Id <= 0)
      {
        return result;
      }

      var administrator = await _dbContext.Administrators
        .FirstOrDefaultAsync(a => a.AdminId == request.Id);

      if (administrator == null)
      {
        return result;
      }

      _dbContext.Administrators.Remove(administrator);
      await _dbContext.SaveChangesAsync();

      return result;
    }
  }
}
