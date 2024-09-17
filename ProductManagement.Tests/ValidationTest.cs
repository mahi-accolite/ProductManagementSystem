using ProductManagement.Validations;
namespace ProductManagement.Tests
{
    public class ValidationTest
    {
        private Validate.ValidationBuilder builder;

        public ValidationTest()
        {
            builder = Validate.Begin();
        }
        private void AssertValidationException(string expectedMessage)
        {
            var exception = Assert.Throws<Validate.ValidationException>(() => builder.Check());
            Assert.Contains(expectedMessage, exception.Message);
        }
        [Fact]
        public void IsNotNull_ShouldAddError_WhenValueIsNull()
        {
            // Act
            builder.IsNotNull(null, "TestProperty");

            // Assert
            AssertValidationException("TestProperty cannot be null.");
        }

        [Fact]
        public void IsNotEmpty_ShouldAddError_WhenValueIsEmpty()
        {
            // Act
            builder.IsNotEmpty("   ", "TestProperty");

            // Assert
            AssertValidationException("TestProperty cannot be empty.");
        }

        [Fact]
        public void Min_ShouldAddError_WhenValueIsLessThanMinValue()
        {
            // Act
            builder.Min(5, "TestProperty", 10);

            // Assert
            AssertValidationException("TestProperty value should be greater than: 10");
        }

        [Fact]
        public void Check_ShouldNotThrowException_WhenNoErrors()
        {
            // Act
            builder.IsNotNull(new object(), "TestProperty")
                   .IsNotEmpty("NotEmpty", "TestProperty")
                   .Min(10, "TestProperty", 5)
                   .Check();
        }
    }
}
