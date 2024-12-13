
using FluentValidation;
using FluentValidation.AspNetCore;
using Restaurants.Application.Restaurants.Commads.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandValidators : AbstractValidator<CreateRestaurantCommand>
    {
        private readonly List<string> validCategories = ["Indian", "British"];
        public CreateRestaurantCommandValidators()
        {
            RuleFor(dto => dto.Name)
                .Length(3, 100);
            RuleFor(dto => dto.Category)
                .Must(validCategories.Contains)
                .WithMessage("Invalid category");

            RuleFor(dto => dto.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(dto => dto.Category)
                .NotEmpty()
                .WithMessage("Category is required");
            RuleFor(dto => dto.ContactEmail)
                .EmailAddress()
                .WithMessage("PLease provide a valid email address");

            RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX)");
        }
    }
}
