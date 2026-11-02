using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DosyaYonetimPortali.Services;
using DosyaYonetimPortali.Models.Entities;
using DosyaYonetimPortali.Repositories;

namespace DosyaYonetimPortali.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IGenericRepository<FileItem> _fileRepository;
        private readonly IGenericRepository<Category> _categoryRepository;

        public FileController(
            IFileService fileService,
            IGenericRepository<FileItem> fileRepository,
            IGenericRepository<Category> categoryRepository)
        {
            _fileService = fileService;
            _fileRepository = fileRepository;
            _categoryRepository = categoryRepository;
        }

        // Listeleme, Arama ve Filtreleme
        public async Task<IActionResult> Index(string searchTerm, int? categoryId)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            bool isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            var files = await _fileRepository.GetAllAsync();

            // Admin değilse sadece kendi dosyalarını filtrele
            if (!isAdmin)
            {
                files = files.Where(f => f.AppUserId == currentUserId.Value).ToList();
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                files = files.Where(f => f.FileName != null && f.FileName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                files = files.Where(f => f.CategoryId == categoryId.Value).ToList();
            }

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.IsAdmin = isAdmin;

            return View(files);
        }

        // Yükleme Sayfası (GET)
        public async Task<IActionResult> Upload()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View();
        }

        // Yükleme İşlemi (POST)
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int categoryId)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (file != null && file.Length > 0)
            {
                var uploadedFile = await _fileService.UploadFileAsync(file, categoryId, currentUserId.Value);

                if (uploadedFile != null)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu.");
            return View();
        }

        // Dosya Silme İşlemi
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var fileList = await _fileRepository.GetAllAsync();
            var file = fileList.FirstOrDefault(f => f.Id == id);

            if (file != null)
            {
                _fileRepository.Delete(file);
                await _fileRepository.SaveAsync();
            }

            return RedirectToAction("Index");
        }
    }
}