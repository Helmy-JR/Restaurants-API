using MediatR;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommand: IRequest
    {
        public int Id { get; } 

        public DeleteRestaurantCommand(int id)
        {
            Id = id;
        }

    }
}