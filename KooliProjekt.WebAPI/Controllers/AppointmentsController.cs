using System.Threading.Tasks;
using KooliProjekt.Application.Features.Appointments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class AppointmentsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new ListAppointmentsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
