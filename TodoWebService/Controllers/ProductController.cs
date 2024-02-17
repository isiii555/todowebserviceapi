using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using TodoWebService.Models.DTOs.Product;
using TodoWebService.Services.Product;

namespace TodoWebService.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = Guard.Against.Null(productService);
        }

        [HttpGet("all")] 

        public async Task<IEnumerable<Models.Entities.Product>> All()
        {
            return await _productService.GetAllProducts();
        }

        [HttpPost("update")]

        public async Task<ProductDto> Update(CreateProductRequest request,int id)
        {
            return await _productService.UpdateProduct(request, id);
        }

        [HttpPost("create")]

        public async Task<ProductDto> Create(CreateProductRequest request)
        {
            return await _productService.CreateProduct(request);
        }

        [HttpDelete("delete")]

        public async Task<bool> Delete(int id)
        {
            return await _productService.DeleteProduct(id);
        }

        [HttpGet("filterByName")]

        public async Task<IEnumerable<Models.Entities.Product>> FilterByName([FromQuery]string name)
        {
            return await _productService.FilterProductsByName(name);
        }

        [HttpGet("sort")]

        public async Task<IEnumerable<Models.Entities.Product>> Sort(bool isAscending)
        {
            return await _productService.GetAllProductsSorted(isAscending);
        }

    }
}
