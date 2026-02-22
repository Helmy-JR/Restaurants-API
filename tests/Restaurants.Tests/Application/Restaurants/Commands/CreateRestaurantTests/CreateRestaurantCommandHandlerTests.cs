using Moq;
using Xunit;
using AutoMapper;
using FluentAssertions;
using Restaurants.Domain.Entities;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
//dotnet test --filter FullyQualifiedName~CreateRestaurantCommandHandlerTests

namespace Restaurants.Tests.Application.Restaurants.Commands.CreateRestaurantTests
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public async Task Handle_ForValidCommands_ReturnCreatedRestaurant()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
            var mapperMock = new Mock<IMapper>();

            var command = new CreateRestaurantCommand();
            var restaurant = new Restaurant();
            var restaurantDto = new RestaurantDto();

            mapperMock
                .Setup(m => m.Map<Restaurant>(command))
                .Returns(restaurant);

            mapperMock
                .Setup(m => m.Map<RestaurantDto>(restaurant))
                .Returns(restaurantDto);

            var restaurantRepoMock = new Mock<IRestaurantRepository>();

            restaurantRepoMock
                .Setup(repo => repo.Create(It.IsAny<Restaurant>()))
                .Returns(Task.CompletedTask);

            var userContextMock = new Mock<IUserContext>();
            var currUser = new CurrentUser("owner_id","test@tset.com",[],null,null);
            userContextMock
                .Setup(uc => uc.GetCurrentUser())
                .Returns(currUser);


            var commandHandler = new CreateRestaurantCommandHandler
            (
                loggerMock.Object,
                restaurantRepoMock.Object,
                mapperMock.Object,
                userContextMock.Object
            );

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            // 
            result.Should().Be(restaurantDto);
            restaurant.OwnerId.Should().Be("owner_id");
            restaurantRepoMock.Verify(repo => repo.Create(restaurant),Times.Once);

        }
    }
}