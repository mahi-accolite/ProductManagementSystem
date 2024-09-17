using ProductManagement.Data.DTO;
using ProductManagement.Services.Models.Validators;
using ProductManagement.Validations;

namespace ProductManagement.Tests
{
    public class ProductValidatorTests
    {
        private readonly ProductValidator _validator;
        public ProductValidatorTests()
        {
            _validator = new ProductValidator();
        }

        [Fact]
        public void Check_ShouldNotThrowException_WhenProductIsValid()
        {
            // Arrange
            var product = new ProductDto
            {
                Name = "Valid Product",
                Price = 10.0m,
                StockQuantity = 5
            };

            // Act & Assert
            var exception = Record.Exception(() => _validator.Check(product));
            Assert.Null(exception);
        }

        [Fact]
        public void Check_ShouldThrowException_WhenProductIsNull()
        {
            // Arrange
            ProductDto product = null;

            // Act & Assert
            var exception = Assert.Throws<Validate.ValidationException>(() => _validator.Check(product));
            Assert.Contains("product cannot be null.", exception.Message);
        }

        [Fact]
        public void Check_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            var product = new ProductDto
            {
                Name = string.Empty,
                Price = 10.0m,
                StockQuantity = 5
            };

            // Act & Assert
            var exception = Assert.Throws<Validate.ValidationException>(() => _validator.Check(product));
            Assert.Contains("Name cannot be empty.", exception.Message);
        }

        [Fact]
        public void Check_ShouldThrowException_WhenPriceIsBelowMinimum()
        {
            // Arrange
            var product = new ProductDto
            {
                Name = "Valid Product",
                Price = 0.5m, // Below minimum price of 1
                StockQuantity = 5
            };

            // Act & Assert
            var exception = Assert.Throws<Validate.ValidationException>(() => _validator.Check(product));
            Assert.Contains("Price value should be greater than: 1", exception.Message);
        }
        [Fact]
        public void Min_ShouldAddError_WhenDecimalValueIsLessThanMinValue()
        {
            // Arrange
            var builder = Validate.Begin();

            // Act
            builder.Min(5.5m, "TestProperty", 10);

            // Assert
            var exception = Assert.Throws<Validate.ValidationException>(() => builder.Check());
            Assert.Contains("TestProperty value should be greater than: 10", exception.Message);
        }

        [Fact]
        public void Check_ShouldThrowException_WhenStockQuantityIsBelowMinimum()
        {
            // Arrange
            var product = new ProductDto
            {
                Name = "Valid Product",
                Price = 10.0m,
                StockQuantity = 0 // Below minimum stock quantity of 1
            };

            // Act & Assert
            var exception = Assert.Throws<Validate.ValidationException>(() => _validator.Check(product));
            Assert.Contains("StockQuantity value should be greater than: 1", exception.Message);
        }
    }
}

