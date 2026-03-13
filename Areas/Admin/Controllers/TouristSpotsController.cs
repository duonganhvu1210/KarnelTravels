using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarnelTravels.Data;
using KarnelTravels.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace KarnelTravels.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TouristSpotsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public TouristSpotsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Admin/TouristSpots
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            // F161: Thống kê tổng số điểm du lịch
            ViewData["TotalSpots"] = await _context.TouristSpots.CountAsync();

            var spots = from s in _context.TouristSpots
                        .Include(s => s.Category)
                        select s;

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(searchString))
            {
                spots = spots.Where(s => s.Name.Contains(searchString));
            }

            // Phân trang - 10 items per page
            int pageSize = 10;
            var paginatedSpots = await PaginatedList<TouristSpot>.CreateAsync(
                spots.OrderByDescending(s => s.CreatedAt), 
                pageNumber, 
                pageSize
            );

            ViewData["CurrentSearch"] = searchString;
            return View(paginatedSpots);
        }

        // GET: /Admin/TouristSpots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spot = await _context.TouristSpots
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.SpotId == id);

            if (spot == null)
            {
                return NotFound();
            }

            return View(spot);
        }

        // GET: /Admin/TouristSpots/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = await _context.TouristSpotCategories
                .Where(c => c.IsActive)
                .ToListAsync();
            return View();
        }

        // POST: /Admin/TouristSpots/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpotId,Name,Description,Address,Region,SpotType,Rating,Activities,BestTime,TicketPrice,PriceRange,IsPopular,IsActive,CategoryId")] TouristSpot spot, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload hình ảnh
                if (imageFile != null && imageFile.Length > 0)
                {
                    spot.ImageUrl = await UploadImage(imageFile);
                }

                spot.CreatedAt = DateTime.Now;
                _context.Add(spot);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm điểm du lịch thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = await _context.TouristSpotCategories
                .Where(c => c.IsActive)
                .ToListAsync();
            return View(spot);
        }

        // GET: /Admin/TouristSpots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spot = await _context.TouristSpots.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = await _context.TouristSpotCategories
                .Where(c => c.IsActive)
                .ToListAsync();
            return View(spot);
        }

        // POST: /Admin/TouristSpots/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpotId,Name,Description,Address,Region,SpotType,Rating,Activities,BestTime,TicketPrice,PriceRange,IsPopular,IsActive,CategoryId,ImageUrl,CreatedAt")] TouristSpot spot, IFormFile? imageFile)
        {
            if (id != spot.SpotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý upload hình ảnh mới
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Xóa hình cũ nếu có
                        if (!string.IsNullOrEmpty(spot.ImageUrl))
                        {
                            DeleteOldImage(spot.ImageUrl);
                        }
                        spot.ImageUrl = await UploadImage(imageFile);
                    }

                    _context.Update(spot);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật điểm du lịch thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TouristSpotExists(spot.SpotId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = await _context.TouristSpotCategories
                .Where(c => c.IsActive)
                .ToListAsync();
            return View(spot);
        }

        // GET: /Admin/TouristSpots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spot = await _context.TouristSpots
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.SpotId == id);

            if (spot == null)
            {
                return NotFound();
            }

            return View(spot);
        }

        // POST: /Admin/TouristSpots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spot = await _context.TouristSpots.FindAsync(id);
            if (spot != null)
            {
                // Xóa hình ảnh
                if (!string.IsNullOrEmpty(spot.ImageUrl))
                {
                    DeleteOldImage(spot.ImageUrl);
                }

                _context.TouristSpots.Remove(spot);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa điểm du lịch thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper: Kiểm tra tồn tại
        private bool TouristSpotExists(int id)
        {
            return _context.TouristSpots.Any(s => s.SpotId == id);
        }

        // Helper: Upload hình ảnh
        private async Task<string> UploadImage(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "tourist-spots");
            
            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/tourist-spots/" + uniqueFileName;
        }

        // Helper: Xóa hình ảnh cũ
        private void DeleteOldImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string filePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }

    // Class hỗ trợ phân trang
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(
                (pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
