using System.ComponentModel.DataAnnotations;

namespace KarnelTravels.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? CuisineType { get; set; }

        [StringLength(50)]
        public string? PriceRange { get; set; }

        [StringLength(50)]
        public string? Style { get; set; }

        public TimeSpan? OpenTime { get; set; }

        public TimeSpan? CloseTime { get; set; }

        [StringLength(500)]
        public string? Menu { get; set; }

        public bool HasReservation { get; set; } = true;

        public decimal? Rating { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Resort
    {
        [Key]
        public int ResortId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? ResortType { get; set; }

        public int StarRating { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        [StringLength(500)]
        public string? Amenities { get; set; }

        [StringLength(500)]
        public string? Activities { get; set; }

        [StringLength(500)]
        public string? ComboPackages { get; set; }

        public decimal? Rating { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<ResortRoom> Rooms { get; set; } = new List<ResortRoom>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public class ResortRoom
    {
        [Key]
        public int RoomId { get; set; }

        public int ResortId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomType { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int AvailableRooms { get; set; }

        [StringLength(500)]
        public string? Amenities { get; set; }

        public bool IsActive { get; set; } = true;

        public Resort? Resort { get; set; }
    }
}
