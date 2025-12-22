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
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListAppointmentsQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }
        /*
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetAppointmentQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }
        */
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveAppointmentCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
        
        // Based on 15.11.2025
        // API endpoint to delete Appointments
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteAppointmentCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
