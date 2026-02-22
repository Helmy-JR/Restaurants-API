using MediatR;

namespace Restaurants.Application.Users.Commands
{
    public class UpdateUserDetailsCommand : IRequest
    {
        public DateTime? DateFOfBirth { get; set; }

        public string? Nationality { get; set; }
    }
}