namespace ProductService.Data.DTOs
{
    public class CreateProductDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public required bool IsAvailable { get; set; } 
    }
}
