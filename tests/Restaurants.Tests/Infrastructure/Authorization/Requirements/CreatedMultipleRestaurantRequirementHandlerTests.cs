using Moq;
using Xunit;
using FluentAssertions;
using Restaurants.Domain.Entities;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

//dotnet test --filter FullyQualifiedName~CreatedMultipleRestaurantRequirementHandlerTests
namespace Restaurants.Tests.Infrastructure.Authorization.Requirements
{
    public class CreatedMultipleRestaurantRequirementHandlerTests
    {
        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_shouldSucceed()
        {
            // Arrange
            var CurrentUser = new CurrentUser("1","test@test.com",[],null,null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m=> m.GetCurrentUser()).Returns(CurrentUser);

            var restaurants = new List<Restaurant>
            {
                new () { OwnerId = CurrentUser.Id },
                new () { OwnerId = CurrentUser.Id },
                new () { OwnerId = "2" }
            };

            var restaurantsRepoMock = new Mock<IRestaurantRepository>();
            restaurantsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantRequirement(2);
            var handler = new CreatedMultipleRestaurantRequirementHandler(restaurantsRepoMock.Object , userContextMock.Object);

            var context = new AuthorizationHandlerContext(new[] { requirement }, null, null);

            // Act
            await handler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_shouldFail()
        {
            // Arrange
            var CurrentUser = new CurrentUser("1","test@test.com",[],null,null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m=> m.GetCurrentUser()).Returns(CurrentUser);

            var restaurants = new List<Restaurant>
            {
                new () { OwnerId = CurrentUser.Id },
                new () { OwnerId = "2" }
            };

            var restaurantsRepoMock = new Mock<IRestaurantRepository>();
            restaurantsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantRequirement(2);
            var handler = new CreatedMultipleRestaurantRequirementHandler(restaurantsRepoMock.Object , userContextMock.Object);

            var context = new AuthorizationHandlerContext(new[] { requirement }, null, null);

            // Act
            await handler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeFalse();
        }
    }
}