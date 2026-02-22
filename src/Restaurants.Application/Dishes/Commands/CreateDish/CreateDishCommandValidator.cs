using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
    {
        public CreateDishCommandValidator()
        {
            RuleFor(dish => dish.Name)
                .NotEmpty()
                .WithMessage("Name is required and cannot exceed 100 characters.")
                .MaximumLength(100);

            RuleFor(dish => dish.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(dish => dish.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be a non-negative number.");

            RuleFor(dish => dish.KiloCalories)
                .GreaterThan(0)
                .WithMessage("KiloCalories must be a non-negative number.");
        }
    }
}