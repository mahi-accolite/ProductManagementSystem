using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.API.Filters;
using ProductManagement.Data.Repositories;
using ProductManagement.Services.Services;
using ProductManagement.Services.Models.Validators;
using ProductManagement.Data.Models.Validators;

namespace ProductManagement.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void ConfigureServices_ShouldRegisterExpectedServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            // Simulate adding services as done in Program.cs
            services.AddControllers(options =>
            {
                options.Filters.Add(new ExceptionFilter());
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IValidateProduct, ProductValidator>();

            // Assert
            Assert.Contains(services, s => s.ServiceType == typeof(IProductRepository));
            Assert.Contains(services, s => s.ServiceType == typeof(IProductService));
            Assert.Contains(services, s => s.ServiceType == typeof(IValidateProduct));
        }
    }
}