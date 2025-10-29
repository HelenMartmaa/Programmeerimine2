using System.Threading.Tasks;
using KooliProjekt.Application.Features.Administrators;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class AdministratorsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public AdministratorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new ListAdministratorsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
