
using FluentValidation;
using FluentValidation.AspNetCore;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Commands.EditRestaurant
{
    public class EditRestaurantCommandValidators : AbstractValidator<EditRestaurantCommand>
    {
        public EditRestaurantCommandValidators()
        {
            RuleFor(dto => dto.Name)
                .Length(3, 100);

        }
    }
}
