using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
    {
        private int[] allowedPageSizes = [ 5, 10, 15, 20, 50 ];
        private string[] allowedSortByColumnNames = [ nameof(RestaurantDto.Name),
            nameof(RestaurantDto.Description), nameof(RestaurantDto.Category) ];
        public GetAllRestaurantsQueryValidator()
        {
            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);
            
            RuleFor(r => r.PageSize)
                .Must(value => allowedPageSizes.Contains(value))
                .WithMessage($"PageSize must be in: [{string.Join(", ", allowedPageSizes)}]");
            
            RuleFor(r => r.SortBy)
                .Must(value => allowedSortByColumnNames.Contains(value))
                .When(q => q.SortBy != null)
                .WithMessage($"SortBy is Optional, or must be in: [{string.Join(", ", allowedSortByColumnNames)}]");
            
        }
    }
}