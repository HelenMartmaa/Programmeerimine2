using System.Threading.Tasks;
using KooliProjekt.Application.Features.DoctorUnavailabilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class DoctorUnavailabilitiesController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public DoctorUnavailabilitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListDoctorUnavailabilitiesQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }
        /*
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetDoctorUnavailabilityQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }
        */
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveDoctorUnavailabilityCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
        
        // Based on 15.11.2025
        // API endpoint to delete DoctorUnavailability
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteDoctorUnavailabilityCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
