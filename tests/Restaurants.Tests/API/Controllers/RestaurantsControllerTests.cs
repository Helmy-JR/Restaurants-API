using Moq;
using Xunit;
using System.Net;
using FluentAssertions;
using System.Net.Http.Json;
using Restaurants.Domain.Entities;
using Microsoft.AspNetCore.TestHost;
using Restaurants.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Restaurants.Application.Restaurants.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection.Extensions;

// dotnet test --filter FullyQualifiedName~RestaurantsControllerTests

namespace Restaurants.Tests.API.Controllers
{
    public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock = new();

        public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvalutor>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantRepository),
                                                    _ => _restaurantRepositoryMock.Object));
                });
            });
        }

        [Fact]
        public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
        {
            // Arrange
            var id = 1123;
            _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id))
                                    .ReturnsAsync((Restaurant?)null);


            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/api/restaurants/{id}");

            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ForExistingId_ShouldReturn200OK()
        {
            // Arrange
            var id = 33;

            var restaurant = new Restaurant()
            {
                Id = id,
                Name = "Test Restaurant",
                Description = "Test Description",
            };

            _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id))
                                    .ReturnsAsync(restaurant);


            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/api/restaurants/{id}");
            var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            restaurantDto.Should().NotBeNull();
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Description.Should().Be(restaurant.Description);
        }

        [Fact]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync("/api/restaurants");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
    }
}