using System.Threading.Tasks;
using KooliProjekt.Application.Features.Doctors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class DoctorsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new ListDoctorsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
