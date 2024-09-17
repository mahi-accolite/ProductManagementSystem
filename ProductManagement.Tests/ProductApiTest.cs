using Moq;
using ProductManagement.Data.DTO;
using ProductManagement.Services.Services;
using ProductManagement.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data.Domain;

namespace ProductManagement.Tests
{
    public class ProductApiTest : IDisposable
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductController _controller;
        private readonly Product product;
        private readonly List<ProductDto> record;

        public ProductApiTest()
        {
            // Initialize mocks
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
            product = new Product();
            record = product.ProductDTOList();
            
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOkResult_WithProducts()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetAllProducts()).ReturnsAsync(record);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(record.Count, returnedProducts.Count);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnOkResult_WithProduct()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductById(record[0].Id)).ReturnsAsync(record[0]);

            // Act
            var result = await _controller.GetProduct(record[0].Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(record[0].Id, returnedProduct.Id);
            Assert.Equal("Product 1", returnedProduct.Name);
        }
        [Fact]
        public async Task UpdateProduct_ShouldReturnOkResult()
        {
            // Arrange
            _mockProductService.Setup(service => service.UpdateProduct(record[0], record[0].Id));

            // Act
            var result = await _controller.UpdateProduct(record[0], record[0].Id);

            // Assert
            Assert.IsType<OkResult>(result);
        }



        [Fact]
        public async Task AddProduct_ValidProductDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "New Product",
                Price = 10.0m,
                StockQuantity = 5
            };

            var createdProduct = new ProductRecord
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity
            };

            // Setup the mock to return the created product
            _mockProductService.Setup(service => service.CreateProduct(productDto))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.AddProduct(productDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetProduct", actionResult.ActionName);
            Assert.Equal(createdProduct.Id, actionResult.RouteValues["id"]);
            Assert.Equal(createdProduct, actionResult.Value);
        }
        public void Dispose()
        {
            _mockProductService.VerifyAll();
        }
    }
}
