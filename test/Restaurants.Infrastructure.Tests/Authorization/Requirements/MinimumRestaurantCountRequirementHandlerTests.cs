using Xunit;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.EditRestaurant;
using Microsoft.Extensions.Logging;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests
{
    public class MinimumRestaurantCountRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_shouldSucceedAsync()
        {

            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2",
                },
            };
            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(restaurants.AsEnumerable());
            var requirement = new MinimumRestaurantCountRequirement(2);
            var handler = new MinimumRestaurantCountRequirementHandler( userContextMock.Object, restaurantRepositoryMock.Object);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // act
            await handler.HandleAsync(context);


            // assert

            context.HasSucceeded.Should().Be(true);
        }


        [Fact()]
        public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_shouldFail()
        {

            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2",
                },
            };
            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(restaurants.AsEnumerable());
            var requirement = new MinimumRestaurantCountRequirement(2);
            var handler = new MinimumRestaurantCountRequirementHandler(userContextMock.Object, restaurantRepositoryMock.Object);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // act
            await handler.HandleAsync(context);


            // assert

            context.HasSucceeded.Should().Be(false);
        }

    }
}