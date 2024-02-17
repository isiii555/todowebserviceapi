namespace TodoWebService.Models.DTOs.Product
{
    public class ProductDto
    {
        public ProductDto(string name, string description, double price, int id)
        {
            Name = name;
            Description = description;
            Price = price;
            Id = id;
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
    }
}
