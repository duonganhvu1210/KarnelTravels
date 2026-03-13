using System.ComponentModel.DataAnnotations;

namespace KarnelTravels.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public int? UserId { get; set; }
        public int? TourId { get; set; }
        public int? HotelId { get; set; }
        public int? ResortId { get; set; }
        public int? TransportId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string CustomerEmail { get; set; } = string.Empty;

        [StringLength(20)]
        public string? CustomerPhone { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public DateTime? TravelDate { get; set; }

        public int NumberOfPeople { get; set; } = 1;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public string? PaymentMethod { get; set; }

        public bool IsPaid { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public User? User { get; set; }
        public Tour? Tour { get; set; }
        public Hotel? Hotel { get; set; }
        public Resort? Resort { get; set; }
        public Transport? Transport { get; set; }
    }

    public class Promotion
    {
        [Key]
        public int PromotionId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public int DiscountPercent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(50)]
        public string? ApplyTo { get; set; }

        public bool IsActive { get; set; } = true;

        public bool ShowOnHome { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? ServiceInterest { get; set; }

        public DateTime? PreferredDate { get; set; }

        public int? NumberOfPeople { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        public int? Rating { get; set; }

        [StringLength(200)]
        public string? Title { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int? UserId { get; set; }
        public int? HotelId { get; set; }
        public int? ResortId { get; set; }
        public int? RestaurantId { get; set; }
        public int? TourId { get; set; }
        public int? TouristSpotId { get; set; }

        [Required]
        public int Rating { get; set; }

        [StringLength(2000)]
        public string? Comment { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
    }
}
