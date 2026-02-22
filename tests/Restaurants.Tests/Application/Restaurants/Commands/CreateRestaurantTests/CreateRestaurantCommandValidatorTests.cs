using Xunit;
using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Validators;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
//dotnet test --filter FullyQualifiedName~CreateRestaurantCommandValidatorTests


namespace Restaurants.Tests.Application.Restaurants.Commands.CreateRestaurantTests
{
    public class CreateRestaurantCommandValidatorTests
    {
        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Test Restaurant",
                Category = "Italian",
                ContactEmail = "Test@gmail.com",
                PostalCode = "12-345",
            };

            var validator = new CreateRestaurantCommandValidator();
            
            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        // TestMethod_Scenario_ExcpectedResult
        public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Te",
                Category = "German",
                ContactEmail = "@gmail.com",
                PostalCode = "12345",
            };

            var validator = new CreateRestaurantCommandValidator();
            
            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name);
            result.ShouldHaveValidationErrorFor(c => c.Category);
            result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }


        [Theory]
        [InlineData("Italian")]
        [InlineData("Mexican")]
        [InlineData("Japanese")]
        [InlineData("American")]
        [InlineData("Indian")]
        // TestMethod_Scenario_ExcpectedResult
        public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategory(string category)
        {
            // Arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand{Category = category};

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Category);
        }


        [Theory]
        [InlineData("10220")]
        [InlineData("1234")]
        [InlineData("12-34")]
        [InlineData("12-3456")]
        // TestMethod_Scenario_ExcpectedResult
        public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPostalCode(string postalCode)
        {
            // Arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand{PostalCode = postalCode};

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }
    }
}