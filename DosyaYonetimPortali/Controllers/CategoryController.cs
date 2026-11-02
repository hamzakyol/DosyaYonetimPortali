using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DosyaYonetimPortali.Models.Entities;
using DosyaYonetimPortali.Repositories;

namespace DosyaYonetimPortali.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryController(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Kategorileri Listele
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        // Kategori Ekleme (POST)
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var category = new Category
                {
                    Name = name,
                    CreatedAt = DateTime.Now,
                    AppUserId = currentUserId.Value
                };

                await _categoryRepository.AddAsync(category);
                await _categoryRepository.SaveAsync();
            }

            return RedirectToAction("Index");
        }

        // Kategori Silme
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                _categoryRepository.Delete(category);
                await _categoryRepository.SaveAsync();
            }

            return RedirectToAction("Index");
        }
    }
}