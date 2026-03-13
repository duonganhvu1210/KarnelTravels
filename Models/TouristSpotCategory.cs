using System.ComponentModel.DataAnnotations;

namespace KarnelTravels.Models
{
    public class TouristSpotCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TouristSpot> TouristSpots { get; set; } = new List<TouristSpot>();
    }
}
