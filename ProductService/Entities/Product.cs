using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } 

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public bool? IsAvailable { get; set; } = true;

        [Required]
        public required string CreatorUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
