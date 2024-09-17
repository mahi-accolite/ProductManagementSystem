using Moq;
using ProductManagement.Data.Domain;
using ProductManagement.Data.DTO;
using ProductManagement.Data.Exceptions;
using ProductManagement.Data.Mappers;
using ProductManagement.Data.Models.Validators;
using ProductManagement.Data.Repositories;
using ProductManagement.Services.Services;

namespace ProductManagement.Tests
{
    public class ProductServiceTests : IDisposable
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;
        private List<ProductRecord> _testProducts;
        private readonly Mock<IValidateProduct> _validator;
        private readonly Product product;
        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _validator = new Mock<IValidateProduct>();
            _productService = new ProductService(_mockProductRepository.Object, _validator.Object);
            product = new Product();
            _testProducts = product.ProductRecordList();// Call the setup method to initialize test data                        
        }
      
        [Fact]
        public async Task GetAllProducts_ReturnsAllProducts()
        {
            // Arrange            
            _mockProductRepository.Setup(repo => repo.GetAll()).ReturnsAsync(_testProducts);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.Equal(_testProducts.Count(), result.Count());
            Assert.All(result, product => Assert.IsType<ProductDto>(product));
        }

        [Fact]
        public async Task GetProductById_ValidId_ReturnsProductDto()
        {
            // Arrange
            var productId = _testProducts[0].Id; // Use the ID of the first test product
            var productRecord = _testProducts[0];
            var productDto = ProductMapper.ToProductDTO(productRecord);

            _mockProductRepository.Setup(repo => repo.GetById(productId)).ReturnsAsync(productRecord);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
        }

        [Fact]
        public async Task GetProductById_InvalidId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidId = new Guid("acba9ac0-10f4-45bc-9172-aecc9178eef1");
            _mockProductRepository.Setup(repo => repo.GetById(invalidId)).ThrowsAsync(new NotFoundException("Product not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _productService.GetProductById(invalidId));
        }
        [Fact]
        public async Task CreateProduct_ShouldThrowException_WhenValidationFails()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Invalid Product", Price = -5.0m }; // Invalid price

            _validator.Setup(v => v.Check(productDto)).Throws(new ArgumentException("Invalid product"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _productService.CreateProduct(productDto));

            // Verify that the repository method was not called
            _mockProductRepository.Verify(r => r.Create(It.Is<ProductRecord>(pr=>pr.Name == productDto.Name)), Times.Never);
        }
        [Fact]       
        public async Task CreateProduct_ValidProductDto_CreatesProduct()
        {
            // Arrange
            var expectedProductRecord = _testProducts.First();
            var productdto = ProductMapper.ToProductDTO(expectedProductRecord);
            _mockProductRepository.Setup(repo => repo.Create(It.Is<ProductRecord>(pr =>
                pr.Name == expectedProductRecord.Name &&
                pr.Price == expectedProductRecord.Price)))
                .ReturnsAsync(expectedProductRecord);
            // Act
            var result = await _productService.CreateProduct(productdto);

            // Assert
            Assert.Equal(expectedProductRecord.Name, result.Name);
            _mockProductRepository.Verify(repo => repo.Create(It.Is<ProductRecord>(pr => pr.Name == productdto.Name && pr.Price == productdto.Price)), Times.Once);
        }
        [Fact]
        public async Task UpdateProduct_InvalidProductDto_ShouldThrowException()
        {
            // Arrange
            var invalidProductDto = new ProductDto 
            { 
                Name = null,  // Assuming Name is required and cannot be null
                Price = -1.0m // Assuming Price must be non-negative
            };
            
            // Setup validation to throw an exception for invalid DTO
            _validator.Setup(v => v.Check(invalidProductDto)).Throws(new ArgumentException("Invalid product data"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.UpdateProduct(invalidProductDto, Guid.NewGuid()));
            
            // Verify that the exception message is as expected
            Assert.Equal("Invalid product data", exception.Message);
            
            // Verify that Update was never called on the repository
            _validator.Verify(v => v.Check(invalidProductDto), Times.Once);
            _mockProductRepository.Verify(repo => repo.Update(It.IsAny<ProductRecord>()), Times.Never);
        }
        [Fact]
        public async Task UpdateProduct_ValidProductDto_UpdatesProduct()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Updated Product",
                Price = 20.0m,
                StockQuantity = 10
            };

            var productId = Guid.NewGuid();

            // Create a new instance of ProductRecord with expected values
            var expectedProductRecord = new ProductRecord
            {
                Id = productId,
                Name = productDto.Name,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity
            };

            // Setup validation to succeed
            _validator.Setup(v => v.Check(productDto));

            // Setup the repository mock to expect an update call with the correct values
            _mockProductRepository.Setup(repo => repo.Update(It.Is<ProductRecord>(pr =>
            pr.Name == expectedProductRecord.Name &&
            pr.Price == expectedProductRecord.Price)))
     .Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateProduct(productDto, productId);

            // Assert
            // Verify that validation was called once with the correct productDto
            _validator.Verify(v => v.Check(productDto), Times.Once);

            // Verify that Update was called once with the correct ProductRecord
            _mockProductRepository.Verify(repo => repo.Update(It.Is<ProductRecord>(pr =>
                pr.Id == expectedProductRecord.Id &&
                pr.Name == expectedProductRecord.Name &&
                pr.Price == expectedProductRecord.Price &&
                pr.StockQuantity == expectedProductRecord.StockQuantity)), Times.Once);
        }
        public void Dispose()
        {
            // Clean up any resources if necessary
            _mockProductRepository?.VerifyAll();
        }
    }
}