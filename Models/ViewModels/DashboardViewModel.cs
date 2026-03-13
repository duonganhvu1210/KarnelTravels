using KarnelTravels.Models;

namespace KarnelTravels.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalSpots { get; set; }
        public int TotalHotels { get; set; }
        public int TotalTours { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int UnreadContacts { get; set; }
        public List<Booking> RecentBookings { get; set; } = new List<Booking>();
    }
}
