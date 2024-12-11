
using MediatR;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commads.DeleteRestaurant
{
    public class DeleteRestaurantCommentHandler(IRestaurantRepository restaurantRepository) : IRequestHandler<DeleteRestaurantCommand, bool>
    {
        public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantRepository.GetByIdAsync(request.Id);
            if (restaurant == null) return false;
            await restaurantRepository.Delete(restaurant);
            return true;
        }

    }
}
