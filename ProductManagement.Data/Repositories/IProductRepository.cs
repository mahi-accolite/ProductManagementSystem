using ProductManagement.Data.Domain;

namespace ProductManagement.Data.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductRecord>> GetAll();
    Task<ProductRecord> GetById(Guid id);
    Task<ProductRecord> Create(ProductRecord record);
    Task Update(ProductRecord record);
}
