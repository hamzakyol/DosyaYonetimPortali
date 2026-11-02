using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DosyaYonetimPortali.Models.Entities;
using DosyaYonetimPortali.Repositories;

namespace DosyaYonetimPortali.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenericRepository<AppUser> _userRepository;

        public AccountController(IGenericRepository<AppUser> userRepository)
        {
            _userRepository = userRepository;
        }

        // Giriş Yap (GET)
        public IActionResult Login()
        {
            return View();
        }

        // Giriş Yap (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email != null && u.Email.Trim().Equals(email.Trim(), StringComparison.OrdinalIgnoreCase) && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Email);

                // admin@test.com e-postasına sahip kullanıcıyı Admin kabul ediyoruz
                bool isAdmin = user.Email.Equals("admin@test.com", StringComparison.OrdinalIgnoreCase);
                HttpContext.Session.SetString("IsAdmin", isAdmin ? "true" : "false");

                return RedirectToAction("Index", "File");
            }

            ViewBag.Error = "E-posta veya şifre hatalı!";
            return View();
        }

        // Kayıt Ol (GET)
        public IActionResult Register()
        {
            return View();
        }

        // Kayıt Ol (POST)
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                var newUser = new AppUser
                {
                    Email = email.Trim(),
                    Password = password,
                    CreatedAt = DateTime.Now
                };

                await _userRepository.AddAsync(newUser);
                await _userRepository.SaveAsync();

                return RedirectToAction("Login");
            }

            ViewBag.Error = "Lütfen tüm alanları doldurunuz.";
            return View();
        }

        // Çıkış Yap
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}