using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();

        Task<Restaurant?> GetByIdAsync(int id);

        Task Create(Restaurant entity);

        Task<int> CreateWithID(Restaurant entity);

        Task DeleteAsync(Restaurant entity);

        Task SaveChanges();
    }
}