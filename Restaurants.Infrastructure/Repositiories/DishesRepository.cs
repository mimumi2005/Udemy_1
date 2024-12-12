

using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance;

namespace Restaurants.Infrastructure.Repositiories
{
    internal class DishesRepository(RestaurantDbContext dbContext) : IDishesRepository
    {

        public async Task<int> Create(Dish entity)
        {
            dbContext.Dishes.Add(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(IEnumerable<Dish> entities)
        {
            foreach (Dish entity in entities)  dbContext.Dishes.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
