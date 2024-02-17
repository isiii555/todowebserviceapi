using System.ComponentModel.DataAnnotations;

namespace TodoWebService.Models.DTOs.Product
{
    public class CreateProductRequest
    {
        [MinLength(5)]
        [Required]
        public string Name { get; set; }

        [MinLength(5)]
        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
