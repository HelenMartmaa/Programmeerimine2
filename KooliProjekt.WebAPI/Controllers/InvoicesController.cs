using System.Threading.Tasks;
using KooliProjekt.Application.Features.Invoices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class InvoicesController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListInvoicesQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }
    }
}
