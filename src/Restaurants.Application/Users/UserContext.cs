using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Restaurants.Application.Users
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor?.HttpContext?.User;

            if(user == null)
            {
                throw new InvalidOperationException("User Context is not present");
            }

            if(user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;

            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);

            var nationality = user.FindFirst(c => c.Type == "Nationality")?.Value;

            var dateOfBirthString = user.FindFirst(c => c.Type == "DateOfBirth")?.Value;

            var DateOfBirth = dateOfBirthString == null
                ? (DateTime?)null 
                : DateTime.ParseExact(dateOfBirthString,"yyyy-MM-dd", null);

           return new CurrentUser(userId, email, roles, nationality, DateOfBirth);
        }
    }
}