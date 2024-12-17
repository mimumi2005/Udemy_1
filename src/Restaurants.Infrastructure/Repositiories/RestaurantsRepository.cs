
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
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

        public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? SortBy, SortDirection sortDirection)
        {

            var searchPhraseLower = searchPhrase?.ToLower();

            var baseQuery = dbContext.Restaurants
                .Where(r => searchPhraseLower == null || r.Name.ToLower().Contains(searchPhraseLower)
                || r.Description.ToLower().Contains(searchPhraseLower));
            var totalCount = await baseQuery.CountAsync();

            if (SortBy != null)
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), r => r.Name },
                    { nameof(Restaurant.Description), r => r.Description },
                    { nameof(Restaurant.Category), r => r.Category },
                };
                var selectedColumn = columnsSelector[SortBy];

                baseQuery = sortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }
            var restaurants = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();


            return (restaurants, totalCount);
        }


        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefaultAsync(x => x.Id == id);
            return restaurant;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
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
