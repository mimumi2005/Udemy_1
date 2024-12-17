
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler(IMapper mapper, IRestaurantRepository restaurantRepository, ILogger<GetRestaurantByIdQueryHandler> logger, IBlobStorageService blobStrogeService ) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
    {
        public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting restaurant {RestaurantId}", request.Id);
            var restaurant = await restaurantRepository.GetByIdAsync(request.Id)
                    ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());  // Fetch the restaurant(s)
            var restaurantsDto = mapper.Map<RestaurantDto>(restaurant);

            restaurantsDto.LogoSasURl = blobStrogeService.GetBlobSasUrl(restaurant.LogoUrl);
            return restaurantsDto;
        }
    }
}
