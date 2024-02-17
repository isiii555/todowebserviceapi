using TodoWebService.Models.DTOs.Product;

namespace TodoWebService.Services.Product
{
    public interface IProductService
    {
        public Task<ProductDto> CreateProduct(CreateProductRequest request);

        public Task<ProductDto> UpdateProduct(CreateProductRequest request,int id);

        public Task<bool> DeleteProduct(int productId);

        public Task<ProductDto> GetProductById(int productId);

        public Task<IEnumerable<Models.Entities.Product>> GetAllProducts();
        public Task<IEnumerable<Models.Entities.Product>> GetAllProductsSorted(bool isAscending);
        public Task<IEnumerable<Models.Entities.Product>> FilterProductsByName(string name);
    }
}
