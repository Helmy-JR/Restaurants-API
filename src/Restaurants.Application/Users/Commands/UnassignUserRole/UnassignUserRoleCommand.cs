using MediatR;

namespace Restaurants.Application.Users.Commands.UnassignUserRole
{
    public class UnassignUserRoleCommand : IRequest
    {
        public string UserEmail { get; set; }
        public string RoleName { get; set; }   
    }
}