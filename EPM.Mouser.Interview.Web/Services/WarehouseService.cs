using EPM.Mouser.Interview.Data;
using EPM.Mouser.Interview.Models;
using EPM.Mouser.Interview.Web.Interfaces.Services;

namespace EPM.Mouser.Interview.Web.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Product?> GetProduct(long id)
        {
            var product = await _warehouseRepository.Get(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAvailableStockProducts()
        {
            var products = await _warehouseRepository.Query(x => x.InStockQuantity > 0);
            return products;

        }
        public async Task<IEnumerable<Product>> GetPublicInStockProducts()
        {
            var products = await _warehouseRepository.Query(x => x.InStockQuantity > 0
                                                              && x.InStockQuantity > x.ReservedQuantity);
            return products;
        }

        public async Task<UpdateResponse> OrderItem(UpdateQuantityRequest updateQuantityRequest)
        {
            var response = new UpdateResponse();
            var result = await GetProduct(updateQuantityRequest);
            if (!result.Success)
            {
                response.Success = result.Success;
                response.ErrorReason = result.ErrorReason;
                return response;
            }

            var product = result.Model;
            product.ReservedQuantity = product.ReservedQuantity + updateQuantityRequest.Quantity;

            if (product.InStockQuantity < product.ReservedQuantity)
            {
                response.ErrorReason = ErrorReason.NotEnoughQuantity;
                response.Success = false;
                return response;
            }

            await _warehouseRepository.UpdateQuantities(product);
            response.Success = true;
            return response;
        }

        public async Task<UpdateResponse> ShipItem(UpdateQuantityRequest updateQuantityRequest)
        {
            var response = new UpdateResponse();
            var result = await GetProduct(updateQuantityRequest);
            if (!result.Success)
            {
                response.Success = result.Success;
                response.ErrorReason = result.ErrorReason;
                return response;
            }

            var product = result.Model;
            product.ReservedQuantity = ((product.ReservedQuantity - updateQuantityRequest.Quantity) <= 0)
                                            ? 0 : product.ReservedQuantity - updateQuantityRequest.Quantity;

            product.InStockQuantity = product.InStockQuantity - updateQuantityRequest.Quantity;

            if (product.InStockQuantity < 0)
            {
                response.ErrorReason = ErrorReason.NotEnoughQuantity;
                response.Success = false;
                return response;
            }

            await _warehouseRepository.UpdateQuantities(product);
            response.Success = true;
            return response;
        }

        public async Task<UpdateResponse> RestockItem(UpdateQuantityRequest updateQuantityRequest)
        {
            var response = new UpdateResponse();
            var result = await GetProduct(updateQuantityRequest);
            if (!result.Success)
            {
                response.Success = result.Success;
                response.ErrorReason = result.ErrorReason;
                return response;
            }

            var product = result.Model;
            product.InStockQuantity = product.InStockQuantity + updateQuantityRequest.Quantity;
            await _warehouseRepository.UpdateQuantities(product);
            response.Success = true;
            return response;
        }

        private async Task<CreateResponse<Product>> GetProduct(UpdateQuantityRequest updateQuantityRequest)
        {
            var response = new CreateResponse<Product>();

            if (updateQuantityRequest.Quantity < 0)
            {
                response.ErrorReason = ErrorReason.QuantityInvalid;
                response.Success = false;
                return response;
            }

            var product = await _warehouseRepository.Get(updateQuantityRequest.Id);
            if (product == null)
            {
                response.ErrorReason = ErrorReason.InvalidRequest;
                response.Success = false;
                return response;
            }
            response.Success = true;
            response.Model = product;
            return response;
        }

        public async Task<CreateResponse<Product>> AddNewProduct(Product product)
        {
            var response = new CreateResponse<Product>();
            if (product.InStockQuantity < 0)
            {
                response.ErrorReason = ErrorReason.QuantityInvalid;
                response.Success = false;
                return response;
            }
            if (string.IsNullOrEmpty(product.Name))
            {
                response.ErrorReason = ErrorReason.InvalidRequest;
                response.Success = false;
                return response;
            }
            var products = await _warehouseRepository.List();
            var list = products.Select(x =>
             {
                 x.Name = x.Name.LastIndexOf("(") > 0 ? x.Name.Substring(0, x.Name.LastIndexOf("(")) : x.Name;
                 return x;
             });

            var existingProductsCount = list.Count(x => x.Name == product.Name.Trim());

            product.Id = products.Count + 1;
            product.ReservedQuantity = 0;
            product.Name = (existingProductsCount == 0) ? product.Name.Trim() : $"{product.Name.Trim()}({existingProductsCount})";
            response.Model = await _warehouseRepository.Insert(product);
            response.Success = true;
            return response;
        }

    }
}
