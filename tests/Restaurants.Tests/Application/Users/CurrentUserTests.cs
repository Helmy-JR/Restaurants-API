using Xunit;
using FluentAssertions;
using Restaurants.Domain.Constants;
using Restaurants.Application.Users;
//dotnet test --filter FullyQualifiedName~CurrentUserTests

namespace Restaurants.Tests.Application.Users
{
    public class CurrentUserTests
    {
        [Theory()]
        [InlineData(UserRoles.Admin)]
        [InlineData(UserRoles.User)]
        // TestMethod_Scenario_ExcpectedResult
        public void IsInRole_WithMatchingRole_shouldReturnTrue(string role)
        {
            // Arrange
            var currentUser = new CurrentUser("1", "Test@test.com",[UserRoles.Admin,UserRoles.User], null, null);
            
            // Act
            var isInRole = currentUser.IsInRole(role);

            // Assert
            isInRole.Should().BeTrue();
        }

        [Fact()]
        // TestMethod_Scenario_ExcpectedResult
        public void IsInRole_WithNoMatchingRole_shouldReturnfalse()
        {
            // Arrange
            var currentUser = new CurrentUser("1", "Test@test.com",[UserRoles.Admin,UserRoles.User], null, null);
            
            // Act
            var isInRole = currentUser.IsInRole(UserRoles.Owner);

            // Assert
            isInRole.Should().BeFalse();
        }

        [Fact()]
        // TestMethod_Scenario_ExcpectedResult
        public void IsInRole_WithNoMatchingRoleCase_shouldReturnfalse()
        {
            // Arrange
            var currentUser = new CurrentUser("1", "Test@test.com",[UserRoles.Admin,UserRoles.User], null, null);
            
            // Act
            var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

            // Assert
            isInRole.Should().BeFalse();
        }
    }
}