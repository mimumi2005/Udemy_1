﻿
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
            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}