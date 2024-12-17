using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.API.Controllers.Tests
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
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantRepository), _ => _restaurantRepositoryMock.Object));
                });
                });
        }
        [Fact()]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // arrange 
            var client = _factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact()]
        public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange 
            var client = _factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/restaurants");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact()]
        public async Task GetById_ForNonExsistingId_ShouldReturn404NotFound()
        {
            // arrange

            
            var id = 1123;

            _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);
            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync($"/api/restaurants/{id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact()]
        public async Task GetById_ForExsistingId_ShouldReturn200OK()
        {
            // arrange


            var id = 99;

            var restaurant = new Restaurant()
            {
                Id = id,
                Name = "Test",
                Description = "Test description"
            };

            _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);
            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync($"/api/restaurants/{id}");
            var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            restaurantDto.Should().NotBeNull();
            restaurantDto.Name.Should().Be("Test");
            restaurantDto.Description.Should().Be("Test description");

        }
        [Fact()]
        public void CreateRestaurantTest()
        {

        }

        [Fact()]
        public void DeleteResturantTest()
        {

        }

        [Fact()]
        public void EditRestaurantTest()
        {

        }
    }
}