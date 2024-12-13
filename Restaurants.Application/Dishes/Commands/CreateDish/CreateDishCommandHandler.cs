﻿

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{

    public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger, IRestaurantRepository restaurantRepository, IDishesRepository dishesRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService, IMapper mapper) : IRequestHandler<CreatDishCommand, int>
    {
        public async Task<int> Handle(CreatDishCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating new dish: {@DishRequest}", request);
            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
                throw new ForbidException();

            var dish = mapper.Map<Dish>(request);
            return await dishesRepository.Create(dish);
        }
    }
}
