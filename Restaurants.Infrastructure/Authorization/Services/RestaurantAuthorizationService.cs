using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services
{
    public class RestaurantAuthorizationService : IRestaurantAuthorizationService
    {
        private readonly ILogger<RestaurantAuthorizationService> _logger;
        private readonly IUserContext _userContext;

        public RestaurantAuthorizationService
        (
            ILogger<RestaurantAuthorizationService> logger,
            IUserContext userContext
        )
        {
            _logger = logger;
            _userContext = userContext;
        }

        public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
        {
            var user = _userContext.GetCurrentUser();
            _logger.LogInformation("Authorizing user {UserEmail} for operation {Operation} on restaurant {RestaurantName}",
                user.Email, resourceOperation, restaurant);

            if(resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.Read)
            {
                _logger.LogInformation("Create/Read operation - successfully authorized");
                return true;
            }

            if(resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
            {
                _logger.LogInformation("Admin user, Delete operation - successfully authorized");
                return true;
            }
    
             if(restaurant.OwnerId == user.Id && 
             (resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Delete))
            {
                _logger.LogInformation("Restaurant owner, Update/Delete operation - successfully authorized");
                return true;
            }
            
            return false;
        }
    }
}