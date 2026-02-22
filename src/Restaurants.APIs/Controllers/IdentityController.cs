using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnassignUserRole;
using Restaurants.Domain.Constants;

namespace Restaurants.APIs.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;
        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("user")]
        [Authorize]
        public async Task<ActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> AssignUserRole(AssignUserRoleCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpDelete("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> UnassignUserRole(UnassignUserRoleCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}