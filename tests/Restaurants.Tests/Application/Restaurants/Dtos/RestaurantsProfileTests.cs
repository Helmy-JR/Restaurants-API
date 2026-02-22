using Xunit;
using AutoMapper;
using FluentAssertions;
using Restaurants.Domain.Entities;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

//dotnet test --filter FullyQualifiedName~RestaurantsProfileTests

namespace Restaurants.Tests.Application.Restaurants.Dtos
{
    public class RestaurantsProfileTests
    {
        private readonly IMapper _mapper;

        public RestaurantsProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RestaurantsProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
                Name = "Test Restaurant",
                Description = "A test restaurant",
                Category = "Test Category",
                HasDelivery = true,ContactEmail = "TestEmail@test.com",
                ContactNumber = "123456789",
                Address = new Address()
                {
                    City = "Test City",
                    Street = "Test Street",
                    PostalCode = "12345"
                }
            };

            // Act
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            // Assert
            restaurantDto.Should().NotBeNull();
            restaurantDto.Id.Should().Be(restaurant.Id);
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Description.Should().Be(restaurant.Description);
            restaurantDto.Category.Should().Be(restaurant.Category);
            restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
            restaurantDto.City.Should().Be(restaurant.Address.City);
            restaurantDto.Street.Should().Be(restaurant.Address.Street);
            restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
        }


        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public void CreateMap_ForCreateRrstaurantCommandToRestaurant_MapsCorrectly()
        {
            // Arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Test Restaurant",
                Description = "A test restaurant",
                Category = "Test Category",
                HasDelivery = true,
                City = "Test City",
                Street = "Test Street",
                PostalCode = "12345",
                ContactEmail = "tst@tst.com",
                ContactNumber = "0123456789"
            };


            // Act
            var restaurant = _mapper.Map<Restaurant>(command);

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(command.Name);
            restaurant.Description.Should().Be(command.Description);
            restaurant.Category.Should().Be(command.Category);
            restaurant.HasDelivery.Should().Be(command.HasDelivery);
            restaurant.Address.Should().NotBeNull();
            restaurant.Address.City.Should().Be(command.City);
            restaurant.Address.Street.Should().Be(command.Street);
            restaurant.Address.PostalCode.Should().Be(command.PostalCode);
            restaurant.ContactEmail.Should().Be(command.ContactEmail);
            restaurant.ContactNumber.Should().Be(command.ContactNumber);
        }

        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public void CreateMap_ForUpdateRrstaurantCommandToRestaurant_MapsCorrectly()
        {
            // Arrange
            var command = new UpdateRestaurantCommand()
            {
                Id = 1,
                Name = "Test Restaurant",
                Description = "A test restaurant",
                HasDelivery = true
            };


            // Act
            var restaurant = _mapper.Map<Restaurant>(command);

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Id.Should().Be(command.Id);
            restaurant.Name.Should().Be(command.Name);
            restaurant.Description.Should().Be(command.Description);
            restaurant.HasDelivery.Should().Be(command.HasDelivery);
        }      
    }
}