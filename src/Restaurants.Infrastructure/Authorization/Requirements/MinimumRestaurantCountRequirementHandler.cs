
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    internal class MinimumRestaurantCountRequirementHandler( IUserContext userContext, IRestaurantRepository restaurantRepository) : AuthorizationHandler<MinimumRestaurantCountRequirement>
    {

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantCountRequirement requirement)
        {
            var currentUser = userContext.GetCurrentUser();

            var restaurants = await restaurantRepository.GetAllAsync();
            int restaurantCount = restaurants.Count(r => r.OwnerId == currentUser!.Id);

            if (restaurantCount >= requirement.MinimumRestaurantCount)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

        }
    }

}
