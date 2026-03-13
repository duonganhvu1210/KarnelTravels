using System.ComponentModel.DataAnnotations;

namespace KarnelTravels.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

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

        public int StarRating { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        [StringLength(500)]
        public string? Amenities { get; set; }

        [StringLength(50)]
        public string? ContactPhone { get; set; }

        [StringLength(200)]
        public string? ContactEmail { get; set; }

        [StringLength(1000)]
        public string? Policies { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<HotelRoom> Rooms { get; set; } = new List<HotelRoom>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public class HotelRoom
    {
        [Key]
        public int RoomId { get; set; }

        public int HotelId { get; set; }

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

        public Hotel? Hotel { get; set; }
    }
}
