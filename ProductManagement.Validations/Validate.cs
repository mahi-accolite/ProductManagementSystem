namespace ProductManagement.Validations
{

    public static class Validate
    {
        public static ValidationBuilder Begin()
        {
            return new ValidationBuilder();
        }

        public class ValidationBuilder
        {
            private readonly List<string> _errors = new List<string>();

            public ValidationBuilder IsNotNull(object value, string propertyName)
            {
                if (value == null)
                {
                    _errors.Add($"{propertyName} cannot be null.");
                }
                return this;
            }

            public ValidationBuilder IsNotEmpty(string value, string propertyName)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _errors.Add($"{propertyName} cannot be empty.");
                }
                return this;
            }
            public ValidationBuilder Min(int value, string propertyName, int minValue)
            {
                if (value < minValue)
                {
                    _errors.Add($"{propertyName} value should be greater than: {minValue}");
                }
                return this;
            }
            public ValidationBuilder Min(decimal value, string propertyName, decimal minValue)
            {
                if (value < minValue)
                {
                    _errors.Add($"{propertyName} value should be greater than: {minValue}");
                }
                return this;
            }


            public ValidationBuilder Check()
            {
                if (_errors.Any())
                {
                    throw new ValidationException(_errors);
                }
                return this;
            }
        }

        public class ValidationException : Exception
        {
            public ValidationException(IEnumerable<string> errors)
                : base(string.Join(", ", errors))
            {
            }
        }
    }
}
