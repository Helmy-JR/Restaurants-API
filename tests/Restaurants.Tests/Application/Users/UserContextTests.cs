using Moq;
using Xunit;
using FluentAssertions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Constants;
using Restaurants.Application.Users;
//dotnet test --filter FullyQualifiedName~UserContextTests

namespace Restaurants.Tests.Application.Users
{
    public class UserContextTests
    {
        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public void getCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUSer()
        {
            // Arrange
            var dateOfBirth = new DateTime(1990, 01, 01);

            var httpContextAccessorMoc = new Mock<IHttpContextAccessor>();

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, "1"),
                new(ClaimTypes.Email, "Test@test.com"),
                new(ClaimTypes.Role, UserRoles.Admin),
                new(ClaimTypes.Role, UserRoles.User),
                new("Nationality", "German"),
                new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims,"TestAuthType"));

            httpContextAccessorMoc.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user 
            });
            
            var userContext = new UserContext(httpContextAccessorMoc.Object);

            // Act
            var currentUser = userContext.GetCurrentUser();

            // Assert
            currentUser.Should().NotBeNull();
            currentUser!.Id.Should().Be("1");
            currentUser.Email.Should().Be("Test@test.com");
            currentUser.Roles.Should().Contain(UserRoles.Admin);
            currentUser.Roles.Should().Contain(UserRoles.User);
            currentUser.Nationality.Should().Be("German");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);
        }


        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public void getCurrentUser_WithUserContextNotpresent_throwsInvalidOperationException()
        {
            // Arrange
            var httpContextAccessorMoc = new Mock<IHttpContextAccessor>();
            httpContextAccessorMoc.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var userContext = new UserContext(httpContextAccessorMoc.Object);

            // Act
            Action act = () => userContext.GetCurrentUser();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("User Context is not present");
        }
    }
}