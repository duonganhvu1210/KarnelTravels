using KarnelTravels.Models;

namespace KarnelTravels.Models.ViewModels
{
    public class DashboardViewModel
    {
        // F161-F164: Thống kê tổng quan
        public int TotalDestinations { get; set; }
        public int TotalHotels { get; set; }
        public int TotalRestaurants { get; set; }
        public int TotalResorts { get; set; }

        // F165: Đơn hàng hôm nay
        public int TodayOrdersCount { get; set; }

        // F166: Doanh thu tháng này
        public decimal MonthlyRevenue { get; set; }

        // F167: Danh sách đơn hàng gần đây
        public List<Booking> RecentBookings { get; set; } = new List<Booking>();

        // F168: Dữ liệu biểu đồ doanh thu 12 tháng
        public List<MonthlyRevenueData> MonthlyRevenues { get; set; } = new List<MonthlyRevenueData>();

        // F169: Liên hệ chưa đọc
        public List<Contact> UnreadContacts { get; set; } = new List<Contact>();

        // F170: Tổng số đơn hàng
        public int TotalBookings { get; set; }

        // Tổng doanh thu
        public decimal TotalRevenue { get; set; }

        // Số lượng Tours
        public int TotalTours { get; set; }

        // Số lượng Transports
        public int TotalTransports { get; set; }
    }

    public class MonthlyRevenueData
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}
