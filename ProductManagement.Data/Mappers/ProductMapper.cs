using ProductManagement.Data.Domain;
using ProductManagement.Data.DTO;

namespace ProductManagement.Data.Mappers;

public static class ProductMapper
{
    public static ProductDto ToProductDTO(ProductRecord productRecord)
    {
        return new ProductDto()
        {
            Id = productRecord.Id,
            Name = productRecord.Name,
            Price = productRecord.Price,
            StockQuantity = productRecord.StockQuantity
        };
    }

    public static ProductRecord ToProductRecord(ProductDto productDto)
    {
        return new ProductRecord()
        {
            Name = productDto.Name,
            Price = productDto.Price,
            StockQuantity = productDto.StockQuantity
        };
    }
    //public static void ToProductDTO(ProductRecord productRecord, ProductDto productDto)
    //{
    //    productDto.Id = productRecord.Id;
    //    productDto.Name = productRecord.Name;
    //    productDto.Price = productRecord.Price;
    //    productDto.StockQuantity = productRecord.StockQuantity;
    //}

    //public static void ToProductRecord(ProductDto productDto, ProductRecord productRecord)
    //{
    //    productRecord.Name = productDto.Name;
    //    productRecord.Price = productDto.Price;
    //    productRecord.StockQuantity = productDto.StockQuantity;
    //}
}
