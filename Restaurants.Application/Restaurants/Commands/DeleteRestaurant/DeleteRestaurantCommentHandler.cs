
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commads.DeleteRestaurant
{
    public class DeleteRestaurantCommentHandler(IRestaurantRepository restaurantRepository, ILogger<DeleteRestaurantCommentHandler> logger) : IRequestHandler<DeleteRestaurantCommand>
    {
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting restaurant with id : {RestaurantId}", request.Id);
            var restaurant = await restaurantRepository.GetByIdAsync(request.Id);
            if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
            await restaurantRepository.Delete(restaurant);
        }

    }
}
