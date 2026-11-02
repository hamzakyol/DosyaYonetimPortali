using DosyaYonetimPortali.Models.Entities;
using DosyaYonetimPortali.Repositories;

namespace DosyaYonetimPortali.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IGenericRepository<FileItem> _fileRepository;

        public FileService(IWebHostEnvironment env, IGenericRepository<FileItem> fileRepository)
        {
            _env = env;
            _fileRepository = fileRepository;
        }

        public async Task<FileItem?> UploadFileAsync(IFormFile file, int categoryId, int userId)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileItem = new FileItem
            {
                FileName = file.FileName,
                FilePath = $"/uploads/{uniqueFileName}",
                FileExtension = extension,
                FileSize = file.Length,
                CategoryId = categoryId,
                AppUserId = userId,
                UploadedAt = DateTime.Now
            };

            await _fileRepository.AddAsync(fileItem);
            await _fileRepository.SaveAsync();

            return fileItem;
        }

        public bool DeleteFileFromDisk(string filePath)
        {
            var absolutePath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
                return true;
            }
            return false;
        }
    }
}