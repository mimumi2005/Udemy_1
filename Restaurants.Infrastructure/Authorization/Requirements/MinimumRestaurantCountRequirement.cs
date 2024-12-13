

using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    public class MinimumRestaurantCountRequirement(int minimumRestaurantCount) : IAuthorizationRequirement
    {
        public int MinimumRestaurantCount { get; } = minimumRestaurantCount;
    }
}
