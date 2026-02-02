using AutoMapper;
using Restaurants.Domain.Entities;
using Restaurants.Application.Dishes.Commands.CreateDish;

namespace Restaurants.Application.Dishes.Dtos
{
    public class DishesProfile : Profile
    {
        public DishesProfile()
        {
            CreateMap<CreateDishCommand, Dish>();
            CreateMap<Dish, DishDto>();
        }
    }
}