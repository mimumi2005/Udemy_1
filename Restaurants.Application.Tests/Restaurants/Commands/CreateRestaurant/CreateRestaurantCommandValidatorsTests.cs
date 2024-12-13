using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commads.EditRestaurant;
using Xunit;


namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandValidatorsTests
    {
        [Fact()]
        public void Validator_ForValidCommand_SHouldNOtHaveValidationErrors()
        {
            // arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Test",
                Category = "Indian",
                ContactEmail = "test@test.com",
                Description = "test",
                PostalCode = "12-345",
            };

            var validator = new CreateRestaurantCommandValidators();

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveAnyValidationErrors();

        }

        [Fact()]
        public void Validator_ForInvalidCommand_SHouldHaveValidationErrors()
        {
            // arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Tt",
                Category = "Italian",
                ContactEmail = "@test.com",
                PostalCode = "12245",
            };

            var validator = new CreateRestaurantCommandValidators();

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.Name);
            result.ShouldHaveValidationErrorFor(c => c.Category);
            result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
            result.ShouldHaveValidationErrorFor(c => c.Description);
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);

        }

        [Theory]
        [InlineData("British")]
        [InlineData("German")]
        [InlineData("Latvian")]
        [InlineData("Indian")]
        public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidators();
            var command = new CreateRestaurantCommand { Category = category };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveValidationErrorFor(c => c.Category);

        }

        [Theory]
        [InlineData("12311")]
        [InlineData("1234123")]
        [InlineData("433-3")]
        [InlineData("10 233")]
        public void Validator_ForInvalidPostalCodes_ShouldHaveValidationErrorsForPostalCodeProperty(string postalCode)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidators();
            var command = new CreateRestaurantCommand { PostalCode = postalCode };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);

        }
    }
}