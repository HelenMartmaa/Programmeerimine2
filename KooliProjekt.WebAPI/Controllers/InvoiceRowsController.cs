using System.Threading.Tasks;
using KooliProjekt.Application.Features.InvoiceRows;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class InvoiceRowsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public InvoiceRowsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new ListInvoiceRowsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
