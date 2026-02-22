namespace Restaurants.Application.Users
{
    public record CurrentUser
    (
        string Id, 
        string Email, 
        IEnumerable<string> Roles,
        string? Nationality,
        DateTime? DateOfBirth
    )
    {
        public bool IsInRole(string role) => Roles.Contains(role);
    }
}