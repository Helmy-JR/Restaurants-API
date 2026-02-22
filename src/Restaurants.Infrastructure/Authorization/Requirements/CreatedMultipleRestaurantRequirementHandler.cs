using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    public class CreatedMultipleRestaurantRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantRequirement>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserContext _userContext;

        public CreatedMultipleRestaurantRequirementHandler
        (
            IRestaurantRepository restaurantRepository,
            IUserContext userContext
        )
        {
            _restaurantRepository = restaurantRepository;
            _userContext = userContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantRequirement requirement)
        {
            var currUser = _userContext.GetCurrentUser(); 

            var restaurants = await _restaurantRepository.GetAllAsync();

            var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currUser.Id);

            if(userRestaurantsCreated >= requirement.MinRestaurantsCreated)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}