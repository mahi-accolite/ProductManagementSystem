using ProductManagement.Data.Domain;
using ProductManagement.Data.Exceptions;
using System.Collections.Concurrent;

namespace ProductManagement.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ConcurrentDictionary<Guid, ProductRecord> products = new ConcurrentDictionary<Guid, ProductRecord>();

        public Task<IEnumerable<ProductRecord>> GetAll()
        {
            var allProducts = products.Values.ToArray();
            return Task.FromResult((IEnumerable<ProductRecord>)allProducts);
        }

        public Task<ProductRecord> GetById(Guid id)
        {
            if (products.TryGetValue(id, out var product))
            {
                return Task.FromResult(product);
            }
            else
            {
                throw new NotFoundException("Product is not available.");
            }
        }

        public Task<ProductRecord> Create(ProductRecord productRecord)
        {
            if (products.TryAdd(productRecord.Id, productRecord))
            {
                return Task.FromResult(productRecord);
            }
            else
            {
                throw new InvalidOperationException("Product already exists.");
            }
        }
        public Task Update(ProductRecord productRecord)
        {
            if (products.ContainsKey(productRecord.Id))
            {
                products[productRecord.Id] = productRecord;
                return Task.CompletedTask;
            }
            else
            {
                throw new NotFoundException("Product is not available.");
            }
        }
    }
}
