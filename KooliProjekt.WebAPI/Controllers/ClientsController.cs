using System.Threading.Tasks;
using KooliProjekt.Application.Features.Clients;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class ClientsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new ListClientsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
