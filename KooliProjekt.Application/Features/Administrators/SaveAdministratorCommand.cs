using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Administrators
{
    public class SaveAdministratorCommand : IRequest<OperationResult>, ITransactional
    {
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public string Department { get; set; }
    }
}
