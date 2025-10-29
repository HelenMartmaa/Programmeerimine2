using System.Threading.Tasks;
using KooliProjekt.Application.Features.AppointmentDocuments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class AppointmentDocumentsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentDocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new ListAppointmentDocumentsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
