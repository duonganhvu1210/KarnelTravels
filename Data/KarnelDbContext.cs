using Microsoft.EntityFrameworkCore;
using KarnelTravels.Models;

namespace KarnelTravels.Data
{
    public class KarnelDbContext : DbContext
    {
        public KarnelDbContext(DbContextOptions<KarnelDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TouristSpot> TouristSpots { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Resort> Resorts { get; set; }
        public DbSet<ResortRoom> ResortRooms { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourSpot> TourSpots { get; set; }
        public DbSet<TourDeparture> TourDepartures { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FullName = "Admin",
                    Email = "admin@karneltravels.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEH encode your password",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            );

            modelBuilder.Entity<TouristSpot>().HasData(
                new TouristSpot { SpotId = 1, Name = "Vịnh Hạ Long", Description = "Vịnh Hạ Long - Di sản thiên nhiên thế giới với hàng nghìn đảo đá vôi", ImageUrl = "/images/spots/ha-long.jpg", Region = "Miền Bắc", SpotType = "Biển", Rating = 4.5m, IsActive = true },
                new TouristSpot { SpotId = 2, Name = "Phú Quốc", Description = "Đảo ngọc Phú Quốc với bãi biển đẹp", ImageUrl = "/images/spots/phu-quoc.jpg", Region = "Miền Nam", SpotType = "Biển", Rating = 4.6m, IsActive = true },
                new TouristSpot { SpotId = 3, Name = "Đà Nẵng", Description = "Thành phố đáng sống với nhiều bãi biển đẹp", ImageUrl = "/images/spots/da-nang.jpg", Region = "Miền Trung", SpotType = "Biển", Rating = 4.7m, IsActive = true },
                new TouristSpot { SpotId = 4, Name = "Sa Pa", Description = "Thị trấn mù sương với cảnh quan núi rừng tuyệt đẹp", ImageUrl = "/images/spots/sapa.jpg", Region = "Miền Bắc", SpotType = "Núi", Rating = 4.4m, IsActive = true },
                new TouristSpot { SpotId = 5, Name = "Huế", Description = "Cố đô Huế với di sản văn hóa phong phú", ImageUrl = "/images/spots/hue.jpg", Region = "Miền Trung", SpotType = "Di tích", Rating = 4.5m, IsActive = true },
                new TouristSpot { SpotId = 6, Name = "Nha Trang", Description = "Thành phố biển xinh đẹp", ImageUrl = "/images/spots/nha-trang.jpg", Region = "Miền Trung", SpotType = "Biển", Rating = 4.6m, IsActive = true }
            );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel { HotelId = 1, Name = "Hotel De La Saigon", Description = "Khách sạn 5 sao sang trọng tại TP.HCM", City = "TP.HCM", StarRating = 5, MinPrice = 1500000, MaxPrice = 5000000, Amenities = "WiFi,Pool,Gym,Spa,Restaurant", IsActive = true },
                new Hotel { HotelId = 2, Name = "Grand Hotel", Description = "Khách sạn 4 sao tại Hà Nội", City = "Hà Nội", StarRating = 4, MinPrice = 800000, MaxPrice = 2500000, Amenities = "WiFi,Restaurant,Parking", IsActive = true },
                new Hotel { HotelId = 3, Name = "Sea View Hotel", Description = "Khách sạn 4 sao view biển Đà Nẵng", City = "Đà Nẵng", StarRating = 4, MinPrice = 900000, MaxPrice = 2800000, Amenities = "WiFi,Pool,Restaurant,Beach", IsActive = true }
            );

            modelBuilder.Entity<Tour>().HasData(
                new Tour { TourId = 1, Name = "Tour Hạ Long 3 ngày 2 đêm", Description = "Khám phá vịnh Hạ Long tuyệt đẹp", Price = 2500000, DurationDays = 3, Destination = "Hạ Long", MaxParticipants = 30, IsFeatured = true, IsActive = true },
                new Tour { TourId = 2, Name = "Tour Phú Quốc 4 ngày 3 đêm", Description = "Nghỉ dưỡng tại đảo ngọc", Price = 4500000, DurationDays = 4, Destination = "Phú Quốc", MaxParticipants = 25, IsFeatured = true, IsActive = true },
                new Tour { TourId = 3, Name = "Tour Đà Nẵng - Hội An 3 ngày", Description = "Khám phá Đà Nẵng và phố cổ Hội An", Price = 3200000, DurationDays = 3, Destination = "Đà Nẵng", MaxParticipants = 30, IsFeatured = true, IsActive = true }
            );

            modelBuilder.Entity<Transport>().HasData(
                new Transport { TransportId = 1, Name = "Xe Khách Hà Nội - Hạ Long", TransportType = "Xe Khách", Route = "Hà Nội - Hạ Long", FromLocation = "Hà Nội", ToLocation = "Hạ Long", Price = 150000, Company = "Karnel Travel", IsActive = true },
                new Transport { TransportId = 2, Name = "Máy bay Sài Gòn - Phú Quốc", TransportType = "Máy bay", Route = "TP.HCM - Phú Quốc", FromLocation = "TP.HCM", ToLocation = "Phú Quốc", Price = 1200000, Company = "Vietnam Airlines", IsActive = true }
            );

            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { RestaurantId = 1, Name = "Nhà hàng Ẩm Thực", Description = "Nhà hàng ẩm thực Việt Nam", City = "Hà Nội", CuisineType = "Việt Nam", PriceRange = "Trung cấp", Style = "Nhà hàng", Rating = 4.2m, IsActive = true },
                new Restaurant { RestaurantId = 2, Name = "Ocean Blue", Description = "Nhà hàng hải sản tươi sống", City = "Đà Nẵng", CuisineType = "Hải sản", PriceRange = "Cao cấp", Style = "Nhà hàng", Rating = 4.5m, IsActive = true }
            );

            modelBuilder.Entity<Resort>().HasData(
                new Resort { ResortId = 1, Name = "Paradise Resort", Description = "Resort 5 sao bãi biển", City = "Phú Quốc", ResortType = "Biển", StarRating = 5, MinPrice = 3000000, MaxPrice = 8000000, Amenities = "Pool,Spa,Gym,Beach,Restaurant", Rating = 4.8m, IsActive = true },
                new Resort { ResortId = 2, Name = "Mountain View Resort", Description = "Resort núi view đẹp", City = "Sa Pa", ResortType = "Núi", StarRating = 4, MinPrice = 1500000, MaxPrice = 4000000, Amenities = "Spa,Restaurant,View", Rating = 4.5m, IsActive = true }
            );

            modelBuilder.Entity<Promotion>().HasData(
                new Promotion { PromotionId = 1, Name = "Giảm giá mùa hè", Description = "Giảm 20% cho tour mùa hè", DiscountPercent = 20, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3), ApplyTo = "Tour", IsActive = true, ShowOnHome = true },
                new Promotion { PromotionId = 2, Name = "Combo khách sạn", Description = "Giảm 15% khi đặt 2 phòng", DiscountPercent = 15, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(2), ApplyTo = "Hotel", IsActive = true, ShowOnHome = true }
            );
        }
    }
}
