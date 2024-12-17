
using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteAll
{
    public class DeleteAllForRestaurantCommand(int restaurantId) : IRequest
    {
        public int RestaurantId { get; } = restaurantId;
    }
}
