﻿

using MediatR;

namespace Restaurants.Application.Restaurants.Commads.DeleteRestaurant
{
    public class DeleteRestaurantCommand(int id) : IRequest
    {
        public int Id { get; } = id;
    }
}
