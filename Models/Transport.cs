using System.ComponentModel.DataAnnotations;

namespace KarnelTravels.Models
{
    public class Transport
    {
        [Key]
        public int TransportId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string TransportType { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Route { get; set; }

        [StringLength(100)]
        public string? FromLocation { get; set; }

        [StringLength(100)]
        public string? ToLocation { get; set; }

        public decimal? Price { get; set; }

        [StringLength(200)]
        public string? Schedule { get; set; }

        [StringLength(200)]
        public string? Company { get; set; }

        [StringLength(500)]
        public string? Amenities { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public class Tour
    {
        [Key]
        public int TourId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        [StringLength(100)]
        public string? Destination { get; set; }

        [StringLength(2000)]
        public string? Itinerary { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(500)]
        public string? IncludedServices { get; set; }

        [StringLength(500)]
        public string? ExcludedServices { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public int MaxParticipants { get; set; }

        public int CurrentParticipants { get; set; }

        public decimal? Rating { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TourSpot> TourSpots { get; set; } = new List<TourSpot>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<TourDeparture> Departures { get; set; } = new List<TourDeparture>();
    }

    public class TourSpot
    {
        [Key]
        public int Id { get; set; }
        public int TourId { get; set; }
        public int SpotId { get; set; }
        public int DayNumber { get; set; }
        public string? Description { get; set; }

        public Tour? Tour { get; set; }
        public TouristSpot? TouristSpot { get; set; }
    }

    public class TourDeparture
    {
        [Key]
        public int DepartureId { get; set; }
        public int TourId { get; set; }
        public DateTime DepartureDate { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; } = true;

        public Tour? Tour { get; set; }
    }
}
