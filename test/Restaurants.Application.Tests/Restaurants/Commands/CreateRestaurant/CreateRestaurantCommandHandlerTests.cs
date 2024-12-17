using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

using Xunit;
using FluentAssertions;

namespace Restaurants.Application.Restaurants.Commads.EditRestaurant.Tests
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ForValidCOmmand_ReturnsCreatedRestaurantId()
        {
            // arrange
            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
            var mapperMock = new Mock<IMapper>();

            var command = new CreateRestaurantCommand();
            var restaurant = new Restaurant();
            mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock.
                Setup(repo => repo.Create(It.IsAny<Restaurant>()))
                .ReturnsAsync(1);

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("Owner-id", "test@test.com", [], null, null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object,
                mapperMock.Object,
                restaurantRepositoryMock.Object,
                userContextMock.Object);

            // act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // assert
            result.Should().Be(1);
            restaurant.OwnerId.Should().Be("Owner-id");
            restaurantRepositoryMock.Verify(r => r.Create(restaurant), Times.Once);
        }
    }
}