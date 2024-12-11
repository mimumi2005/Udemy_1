
using AutoMapper;
using MediatR;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commads.CreateRestaurant
{
    public class CreateRestaurantCommandHandler(IMapper mapper, IRestaurantRepository restaurantRepository) : IRequestHandler<CreateRestaurantCommand, int>
    {
        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = mapper.Map<Restaurant>(request);

            int id = await restaurantRepository.Create(restaurant);
            return id;
        }
    }
}
