using ProductManagement.Data.DTO;

namespace ProductManagement.Data.Models.Validators
{
    public interface IValidateProduct
    {
        void Check(ProductDto product);
    }

}
