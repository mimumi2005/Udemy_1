
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance;
namespace Restaurants.Infrastructure.Repositiories
{
    internal class RestaurantsRepository(RestaurantDbContext dbContext) : IRestaurantRepository
    {
        public async Task<int> Create(Restaurant entity)
        {
            dbContext.Restaurants.Add(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants.ToListAsync();
            return restaurants;
        }
        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefaultAsync(x => x.Id == id);
            return restaurant;
        }
        
        public async Task Delete(Restaurant entity)
        {
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task Edit(Restaurant entity)
        {
            // Attach the entity to the DbContext
            dbContext.Restaurants.Attach(entity);

            // Get the entry for the entity
            var entry = dbContext.Entry(entity);

            // Iterate through properties of the entity
            foreach (var property in entry.Properties)
            {
                // Exclude the Id (primary key) and check for null values
                if (property.Metadata.Name == nameof(entity.Id) || property.CurrentValue == null)
                {
                    // Mark the property as not modified
                    property.IsModified = false;
                }
                else
                {
                    // Mark the property as modified
                    property.IsModified = true;
                }
            }

            // Save changes to the database
            await dbContext.SaveChangesAsync();
        }
    }
}
