namespace ProductService.Data.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool IsAvailable { get; set; }
        public string? CreatorUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
