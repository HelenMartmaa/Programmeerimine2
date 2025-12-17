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
        public async Task<IActionResult> List([FromQuery] ListDoctorUnavailabilitiesQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }
    }
}
