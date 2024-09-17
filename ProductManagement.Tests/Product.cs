using ProductManagement.Data.Domain;
using ProductManagement.Data.DTO;

namespace ProductManagement.Tests
{
    public class Product
    {
        public List<ProductRecord> ProductRecordList()
        {
            return new List<ProductRecord>
            {
                new ProductRecord { Id = new Guid("9e852f95-d087-4f1b-8a53-39c2079f5bcd"), Name = "Product 1", Price = 10.0m, StockQuantity = 100 },
                new ProductRecord { Id = new Guid("a9c5c99a-e9c6-4039-9077-c462789821c0"), Name = "Product 2", Price = 20.0m, StockQuantity = 200 },
                new ProductRecord { Id = new Guid("5f3a705c-73ae-4f2b-a743-585873e753de"), Name = "Product 3", Price = 30.0m, StockQuantity = 300 }
            };

        }
        public List<ProductDto> ProductDTOList()
        {
            return new List<ProductDto>
            {
                new ProductDto { Id = new Guid("9e852f95-d087-4f1b-8a53-39c2079f5bcd"), Name = "Product 1", Price = 10.0m, StockQuantity = 100 },
                new ProductDto { Id = new Guid("a9c5c99a-e9c6-4039-9077-c462789821c0"), Name = "Product 2", Price = 20.0m, StockQuantity = 200 },
                new ProductDto { Id = new Guid("5f3a705c-73ae-4f2b-a743-585873e753de"), Name = "Product 3", Price = 30.0m, StockQuantity = 300 }
            };

        }
    }
}
