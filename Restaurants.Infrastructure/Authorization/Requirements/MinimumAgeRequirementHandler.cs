using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;
        private readonly IUserContext _userContext;

        public MinimumAgeRequirementHandler
        (
            ILogger<MinimumAgeRequirementHandler> logger,
            IUserContext userContext
        )
        {
            _logger = logger;
            _userContext = userContext;
        }

        protected override Task HandleRequirementAsync
        (
            AuthorizationHandlerContext context, 
            MinimumAgeRequirement requirement
        )
        {
            var currUser = _userContext.GetCurrentUser();

            _logger.LogInformation("User: {Email}, Date Of birth: {DOB} - Handling MinimumAgeRequirement ", 
                currUser.Email, currUser.DateOfBirth );
            
            if(currUser.DateOfBirth == null)
            {
                _logger.LogInformation("Date of birth is missing for user {Email}", currUser.Email);
                context.Fail();
                return Task.CompletedTask;
            }
            
            if(currUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateTime.UtcNow)
            {
                _logger.LogInformation("Authorization succeeded for user {Email}", currUser.Email);
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization failed - User: {Email}  does not meet the minimum age requirement.", currUser.Email);
                context.Fail();
            }
            

            return  Task.CompletedTask;
        }
    }
}