using Microsoft.AspNetCore.Http;
using DosyaYonetimPortali.Models.Entities;

namespace DosyaYonetimPortali.Services
{
    public interface IFileService
    {
        Task<FileItem?> UploadFileAsync(IFormFile file, int categoryId, int userId);
        bool DeleteFileFromDisk(string filePath);
    }
}