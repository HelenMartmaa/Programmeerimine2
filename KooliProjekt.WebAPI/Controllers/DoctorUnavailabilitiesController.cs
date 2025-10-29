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
        public async Task<IActionResult> List()
        {
            var query = new ListDoctorUnavailabilitiesQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
