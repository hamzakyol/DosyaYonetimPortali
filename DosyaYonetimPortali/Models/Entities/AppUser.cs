using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace DosyaYonetimPortali.Models.Entities
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Role { get; set; } = "User"; // "Admin" veya "User"

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // İlişkiler
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<FileItem> Files { get; set; } = new List<FileItem>();
    }
}