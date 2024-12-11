
using MediatR;
using AutoMapper;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Commands.EditRestaurant
{
    public class EditRestaurantCommandQuery(IRestaurantRepository restaurantRepository, IMapper mapper) : IRequestHandler<EditRestaurantCommand, bool>
    {
        public async Task<bool> Handle(EditRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantRepository.GetByIdAsync(request.Id);
            if (restaurant == null) return false;


            var editedRestaurant = mapper.Map(request, restaurant);
            await restaurantRepository.Edit(editedRestaurant);
            return true;
        }

    }
}
