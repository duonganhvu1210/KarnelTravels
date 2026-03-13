using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarnelTravels.Data;
using KarnelTravels.Models;

namespace KarnelTravels.Controllers;

public class AccountController : Controller
{
    private readonly KarnelDbContext _context;

    public AccountController(KarnelDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        
        if (user != null && user.PasswordHash == password)
        {
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.FullName);
            return RedirectToAction("Index", "Home");
        }
        
        ViewData["Error"] = "Email hoặc mật khẩu không đúng!";
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                ViewData["Error"] = "Email đã được sử dụng!";
                return View(user);
            }

            user.Role = "Customer";
            user.IsActive = true;
            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }
        return View(user);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Profile()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login");

        var user = await _context.Users.FindAsync(userId.Value);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(User updatedUser)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login");

        var user = await _context.Users.FindAsync(userId.Value);
        if (user != null)
        {
            user.FullName = updatedUser.FullName;
            user.Phone = updatedUser.Phone;
            user.Address = updatedUser.Address;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật thông tin thành công!";
        }
        return RedirectToAction(nameof(Profile));
    }
}
