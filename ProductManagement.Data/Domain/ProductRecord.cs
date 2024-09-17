
namespace ProductManagement.Data.Domain;

public class ProductRecord : BaseRecord
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

}
