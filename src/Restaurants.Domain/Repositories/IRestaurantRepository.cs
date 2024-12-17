﻿
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?>GetByIdAsync(int id);
        Task<int> Create(Restaurant entity);
        Task Delete(Restaurant entity);
        Task Edit(Restaurant entity);
        Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy,SortDirection sortDisrection);

        Task<int> SaveChangesAsync();
    }
}
