using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
    public class DishesRepository : IDishesRepository
    {
        private readonly RestaurantsDbContext _dbContext;

        public DishesRepository(RestaurantsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<int> Create(Dish entity)
        {
            _dbContext.Dishes.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAllDishes(IEnumerable<Dish> entities)
        {
            _dbContext.Dishes.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}