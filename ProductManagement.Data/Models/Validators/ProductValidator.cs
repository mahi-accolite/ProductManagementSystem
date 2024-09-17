using ProductManagement.Data.DTO;
using ProductManagement.Data.Models.Validators;
using ProductManagement.Validations;

namespace ProductManagement.Services.Models.Validators;

public class ProductValidator : IValidateProduct
{
    public void Check(ProductDto product)
    {
        Validate.Begin()
            .IsNotNull(product, nameof(product)).Check()
            .IsNotEmpty(product.Name, nameof(product.Name))
            .Min(product.Price, nameof(product.Price), 1)
            .Min(product.StockQuantity, nameof(product.StockQuantity),1)
            .Check();
    }
}