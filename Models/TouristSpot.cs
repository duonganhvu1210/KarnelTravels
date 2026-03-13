using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarnelTravels.Models
{
    public class TouristSpot
    {
        [Key]
        public int SpotId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        [StringLength(50)]
        public string? SpotType { get; set; }

        public decimal? Rating { get; set; }

        [StringLength(500)]
        public string? Activities { get; set; }

        [StringLength(200)]
        public string? BestTime { get; set; }

        public decimal? TicketPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceRange { get; set; }

        public bool IsPopular { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign Key
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public TouristSpotCategory? Category { get; set; }

        public ICollection<TourSpot> TourSpots { get; set; } = new List<TourSpot>();
    }
}
