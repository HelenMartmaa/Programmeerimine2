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
    public class DeleteAdministratorsCommandHandler : IRequestHandler<DeleteAdministratorsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAdministratorsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAdministratorsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Deletion over relations (hint: CASCADE DELETE)
            //await _dbContext
            //    .ToDoLists
            //    .Where(t => t.Id == request.Id)
            //    .ExecuteDeleteAsync();

            // Deletions with several steps (more than one relation between two tables)
            await _dbContext
                .Administrators
                .Where(t => t.AdminId == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
