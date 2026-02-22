using Moq;
using Xunit;
using AutoMapper;
using FluentAssertions;
using Restaurants.Domain.Entities;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using System.Threading.Tasks;

//dotnet test --filter FullyQualifiedName~UpdateRestaurantCommandHandlerTests
namespace Restaurants.Tests.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandlerTests
    {
         private readonly Mock<ILogger<UpdateRestaurantCommand>> _loggerMock ;
        private readonly Mock<IRestaurantRepository> _repositoryMock ;
        private readonly Mock<IMapper> _mapperMock ;
        private readonly Mock<IRestaurantAuthorizationService> _authorizationMock ;
        private readonly UpdateRestaurantCommandHandler _handler;
        
        public UpdateRestaurantCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateRestaurantCommand>>();
            _repositoryMock = new Mock<IRestaurantRepository>();
            _mapperMock = new Mock<IMapper>();
            _authorizationMock = new Mock<IRestaurantAuthorizationService>();
            _handler = new UpdateRestaurantCommandHandler(
                _loggerMock.Object,
                _repositoryMock.Object,
                _mapperMock.Object,
                _authorizationMock.Object
            );
        }

        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public async Task Handle_WithValidRequest_ShouldUpdateAndSave()
        {
            // Arrange
            var restaurantId = 1;
            var command = new UpdateRestaurantCommand
            {
                Id = restaurantId,
                Name = "New Test",
                Description = "New Description",
                HasDelivery = true,
            };
            var restaurant = new Restaurant
            {
                Id = restaurantId,
                Name = "Test",
                Description = "Test",
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync(restaurant);

            _authorizationMock
            .Setup(a => a.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update))
            .Returns(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.SaveChanges(), Times.Once);
            _mapperMock.Verify(m => m.Map(command,restaurant), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRestaurantDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var restaurantId = 1;
            var request = new UpdateRestaurantCommand { Id = restaurantId };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync((Restaurant?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }


        [Fact]
        public async Task Handle_WhenUserIsUnauthorized_ShouldThrowForbidException()
        {
            // Arrange
            var restaurantId =3;
            var request = new UpdateRestaurantCommand {Id = restaurantId};
            var existingRestaurant = new Restaurant {Id = restaurantId};

            _repositoryMock
                .Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync(existingRestaurant);
            
            _authorizationMock
                .Setup(a => a.Authorize(existingRestaurant, ResourceOperation.Update))
                .Returns(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ForbidException>();
        }
    }
}