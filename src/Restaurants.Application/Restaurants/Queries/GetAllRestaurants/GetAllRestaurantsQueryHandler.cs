
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(IMapper mapper,
        IRestaurantRepository restaurantRepository, ILogger<GetAllRestaurantsQueryHandler> logger): IRequestHandler<GetAllRestaurantsQuery, PageResult<RestaurantDto>>
    {
        public async Task<PageResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all restaurants");
            var (restaurants, totalCount) = await restaurantRepository.GetAllMatchingAsync(request.SearchPhrase
                ,request.PageSize,
                request.PageNumber,
                request.SortBy,
                request.SortDirection);
            var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            var result = new PageResult<RestaurantDto>(restaurantsDto, totalCount, request.PageSize, request.PageNumber);
            return result;
        }
    }
}
