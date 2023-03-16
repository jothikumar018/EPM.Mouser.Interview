using EPM.Mouser.Interview.Models;

namespace EPM.Mouser.Interview.Web.Interfaces.Services
{
    public interface IWarehouseService
    {
        Task<Product?> GetProduct(long id);
        Task<IEnumerable<Product>> GetAvailableStockProducts();
        Task<IEnumerable<Product>> GetPublicInStockProducts();
        Task<UpdateResponse> OrderItem(UpdateQuantityRequest updateQuantityRequest);
        Task<UpdateResponse> ShipItem(UpdateQuantityRequest updateQuantityRequest);
        Task<UpdateResponse> RestockItem(UpdateQuantityRequest updateQuantityRequest);
        Task<CreateResponse<Product>> AddNewProduct(Product product);
    }
}
