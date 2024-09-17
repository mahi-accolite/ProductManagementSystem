using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data.DTO;
using ProductManagement.Services.Services;

namespace ProductManagement.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await productService.GetProductById(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            var product = await productService.CreateProduct(productDto);
            return CreatedAtAction(nameof(GetProduct), new { product.Id }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto, Guid id)
        {
            await productService.UpdateProduct(productDto, id);
            return Ok();
        }
    }
}
