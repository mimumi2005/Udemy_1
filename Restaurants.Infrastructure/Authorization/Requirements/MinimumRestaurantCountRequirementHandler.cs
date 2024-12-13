
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    public class MinimumRestaurantCountRequirementHandler(ILogger<MinimumRestaurantCountRequirementHandler> logger, IUserContext userContext, IRestaurantRepository restaurantRepository) : AuthorizationHandler<MinimumRestaurantCountRequirement>
    {

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantCountRequirement requirement)
        {
            var currentUser = userContext.GetCurrentUser();

            var restaurants = await restaurantRepository.GetAllAsync();
            int restaurantCount = restaurants.Count(r => r.OwnerId == currentUser!.Id);
            logger.LogInformation("User: {Email}, restaurant count {RestaurantCount} - Handling MinimumAgeRequirement",
                currentUser!.Email,
                restaurantCount);

            if (restaurantCount >= requirement.MinimumRestaurantCount)
            {
                logger.LogInformation("Authorization succeeded");
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

        }
    }

}
