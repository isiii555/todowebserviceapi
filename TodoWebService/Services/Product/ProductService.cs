using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs.Product;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly TodoDbContext _todoDbContext;

        public ProductService(TodoDbContext todoDbContext)
        {
            _todoDbContext = Guard.Against.Null(todoDbContext);
        }

        public async Task<ProductDto> CreateProduct(CreateProductRequest request)
        {
            var product = request.Adapt<Models.Entities.Product>();
            var addedProduct = (await _todoDbContext.Products.AddAsync(product)).Entity;
            await _todoDbContext.SaveChangesAsync();
            var productDto = new ProductDto(addedProduct.Name, addedProduct.Description, addedProduct.Price, addedProduct.Id);
            return productDto;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = _todoDbContext.Products.FirstOrDefault(p => p.Id == productId);
            if (product is not null)
            {
                _todoDbContext.Remove(product);
                await _todoDbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<IEnumerable<Models.Entities.Product>> FilterProductsByName(string name)
        {
            var products = await _todoDbContext.Products.Where(p => p.Name.StartsWith(name)).ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Models.Entities.Product>> GetAllProducts()
        {
            return await _todoDbContext.Products.ToListAsync();
        }



        public async Task<IEnumerable<Models.Entities.Product>> GetAllProductsSorted(bool isAscending)
        {
            if (isAscending)
                return await _todoDbContext.Products.OrderBy(p => p.Price).ToListAsync();
            else
                return await _todoDbContext.Products.OrderByDescending(p => p.Price).ToListAsync();
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _todoDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product is not null)
            {
                var productDto = product.Adapt<ProductDto>();
                return productDto;
            }
            throw new Exception("There is no product with entered id.");
        }

        public async Task<ProductDto> UpdateProduct(CreateProductRequest request, int id)
        {
            var product = await _todoDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is not null)
            {
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                _todoDbContext.Products.Update(product);
                await _todoDbContext.SaveChangesAsync();
                var productDto = product.Adapt<ProductDto>();
                return productDto;
            }
            throw new Exception("There is no product with entered id.");
        }
    }
}
