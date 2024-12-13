

using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services
{
    public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
        IUserContext userContext) : IRestaurantAuthorizationService
    {
        public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
        {
            var currentUser = userContext.GetCurrentUser();
            logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantNmae}",
                currentUser.Email,
                resourceOperation,
                restaurant.Name);
            if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Create/read operation - successful authorization");
                return true;
            }
            if (resourceOperation == ResourceOperation.Delete && currentUser.IsInRole(UserRoles.Admin))
            {
                logger.LogInformation("Admin user {UserEmail}, delete operation - successful authorization", currentUser.Email);
                return true;
            }
            if (resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update && currentUser.Id == restaurant.OwnerId)
            {
                logger.LogInformation("Restaurant owner - successful authorization");
                return true;
            }
            return false;
        }
    }
}
