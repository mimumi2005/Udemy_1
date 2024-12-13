using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAll
{
    public class DeleteAllForRestaurantCommandHandler(IMapper mapper, ILogger<DeleteAllForRestaurantCommandHandler> logger,
        IRestaurantAuthorizationService restaurantAuthorizationService, IRestaurantRepository restaurantRepository, IDishesRepository dishesRepository) : IRequestHandler<DeleteAllForRestaurantCommand>
    {
        public async Task Handle(DeleteAllForRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogWarning("Deleting all dishes for restaurant: {restaurantId}", request.RestaurantId);
            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
                throw new ForbidException();

            var dishes = mapper.Map<IEnumerable<Dish>>(restaurant.Dishes);
            await dishesRepository.Delete(dishes);
        }
    }
}
