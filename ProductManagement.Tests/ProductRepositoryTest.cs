using ProductManagement.Data.Domain;
using ProductManagement.Data.Exceptions;
using ProductManagement.Data.Repositories;

namespace ProductManagement.Tests
{
    public class ProductRepositoryTest
    {
        private readonly ProductRepository _mockRepository;
        private readonly Product product;
        private readonly ProductRecord record;
        public ProductRepositoryTest()
        {
            _mockRepository = new ProductRepository();
            product = new Product();
            record = product.ProductRecordList()[0];
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllProducts()
        {
            // Arrange
            var product1 = product.ProductRecordList()[0];
            var product2 = product.ProductRecordList()[1];
            await _mockRepository.Create(product1);
            await _mockRepository.Create(product2);

            // Act
            var products = await _mockRepository.GetAll();

            // Assert
            Assert.Contains(product1, products);
            Assert.Contains(product2, products);
        }

        [Fact]
        public async Task GetById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            await _mockRepository.Create(record);

            // Act
            var result = await _mockRepository.GetById(record.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(record.Id, result.Id);
            Assert.Equal(record.Name, result.Name);
        }

        [Fact]
        public async Task GetById_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _mockRepository.GetById(Guid.NewGuid()));
        }

        [Fact]
        public async Task Create_ShouldAddProductToRepository()
        {
            // Act
            var result = await _mockRepository.Create(record);

            // Assert
            Assert.Equal(record.Id, result.Id);
            Assert.Equal(record.Name, result.Name);
            var products = await _mockRepository.GetAll();
            Assert.Contains(record, products);
        }

        [Fact]
        public async Task Update_ShouldModifyProduct_WhenProductExists()
        {
            // Arrange
            var originalProduct = new ProductRecord { Id = Guid.NewGuid(), Name = "Product" };
            await _mockRepository.Create(originalProduct);
            var updatedProduct = new ProductRecord { Id = originalProduct.Id, Name = "Updated" };
            
            // Act
            await _mockRepository.Update(updatedProduct);

            // Assert
            var result = await _mockRepository.GetById(originalProduct.Id);
            Assert.Equal(updatedProduct.Name, result.Name);
        }

        [Fact]
        public async Task Update_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var product = new ProductRecord { Id = Guid.NewGuid(), Name = "NonExistentProduct" };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _mockRepository.Update(product));
        }
    }
}

