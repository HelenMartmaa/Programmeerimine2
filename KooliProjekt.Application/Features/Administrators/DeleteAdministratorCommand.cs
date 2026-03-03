using MediatR;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Administrators
{
	//Command to delete Administrators
	public class DeleteAdministratorCommand : IRequest<OperationResult>, ITransactional
	{
		public int Id { get; set; }
	}
}
