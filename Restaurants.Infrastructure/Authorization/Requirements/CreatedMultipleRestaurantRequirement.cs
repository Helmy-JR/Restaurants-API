using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    public class CreatedMultipleRestaurantRequirement : IAuthorizationRequirement
    {
        public int MinRestaurantsCreated { get; }

        public CreatedMultipleRestaurantRequirement(int minRestaurantsCreated)
        {
            MinRestaurantsCreated = minRestaurantsCreated;
        }
    }
}