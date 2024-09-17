using ProductManagement.Data.Domain;
using ProductManagement.Data.DTO;

namespace ProductManagement.Services.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProducts();
    Task<ProductDto> GetProductById(Guid id);
    Task<ProductRecord> CreateProduct(ProductDto productDto);
    Task UpdateProduct(ProductDto productDto, Guid id);
}
