using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantsDbContext _dbContext;

        public RestaurantRepository(RestaurantsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await _dbContext.Restaurants.ToListAsync();
            return restaurants;
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(r=> r.Dishes)
                .FirstOrDefaultAsync(r => r.Id == id);
            return restaurant;
        }
        public async Task<int> CreateWithID(Restaurant restaurant)
        {
            _dbContext.Restaurants.Add(restaurant);
            await  _dbContext.SaveChangesAsync();
            return restaurant.Id;
        }
        public Task Create(Restaurant entity)
        {
            _dbContext.Restaurants.Add(entity);
            return _dbContext.SaveChangesAsync();
        }


        public async Task DeleteAsync(Restaurant entity)
        {
            _dbContext.Restaurants.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public Task SaveChanges()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}