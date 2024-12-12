
using MediatR;
using AutoMapper;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Commands.EditRestaurant
{
    public class EditRestaurantCommandQuery(IRestaurantRepository restaurantRepository, IMapper mapper, ILogger<EditRestaurantCommandQuery> logger) : IRequestHandler<EditRestaurantCommand>
    {
        public async Task Handle(EditRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating restaurant with id : {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);
            var restaurant = await restaurantRepository.GetByIdAsync(request.Id);
            if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.Id.ToString());


            var editedRestaurant = mapper.Map(request, restaurant);
            await restaurantRepository.Edit(editedRestaurant);
        }

    }
}
