using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using FluentAssertions;
using Restaurants.Domain.Exceptions;
using System.Security.AccessControl;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Commands.EditRestaurant.Tests
{
    public class EditRestaurantCommandHandlerTests
    {
        private readonly Mock<ILogger<EditRestaurantCommandHandler>> _loggerMock;
        private readonly Mock<IRestaurantRepository> _restarantsRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;


        private readonly EditRestaurantCommandHandler _handler;

        public EditRestaurantCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<EditRestaurantCommandHandler>>();
            _restarantsRepositoryMock = new Mock<IRestaurantRepository>();
            _mapperMock = new Mock<IMapper>();
            _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

            _handler = new EditRestaurantCommandHandler(
                _restarantsRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _restaurantAuthorizationServiceMock.Object);
        }
        [Fact()]
        public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
        {
            // arrange
            var restaurantId = 1;
            var command = new EditRestaurantCommand()
            {
                Id = restaurantId,
                Name = "test",
                Description = "DDesct",
                HasDelivery = false,
            };
            var restaurant = new Restaurant()
            {
                Id = restaurantId,
                Name = "test",
                Description = "Test",
            };
            _restarantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync(restaurant);
            _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
                .Returns(true);

            // act
            await _handler.Handle(command, CancellationToken.None);
            _restarantsRepositoryMock.Verify(r => r.Edit(null), Times.Once);
            _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
        }

        [Fact()]
        public async Task Handle_WithNonExsistingRestaurant_ShouldThrowNotFoundException()
        {
            // arrange 
            var restaurantId = 2;
            var request = new EditRestaurantCommand
            {
                Id = restaurantId
            };
            _restarantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync((Restaurant?)null);

            // act

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Restaurant with id: {restaurantId} doesnt exsist");
        }

        [Fact]
        public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
        {
            // arange 
            var restaurantId = 3;
            var request = new EditRestaurantCommand
            {
                Id = restaurantId
            };

            var exsistingRestaurant = new Restaurant
            {
                Id = restaurantId,
            };

            _restarantsRepositoryMock
                .Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync(exsistingRestaurant);

            _restaurantAuthorizationServiceMock
                .Setup(a => a.Authorize(exsistingRestaurant, ResourceOperation.Update))
                .Returns(false);

            // act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<ForbidException>();
        }

    }
}