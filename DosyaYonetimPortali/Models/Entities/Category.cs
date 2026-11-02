using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DosyaYonetimPortali.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Üst klasör yapısı (Alt klasör mantığı için)
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Category? ParentCategory { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Klasörün sahibi
        public int AppUserId { get; set; }  
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; } = null!;

        // İlişkiler
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<FileItem> Files { get; set; } = new List<FileItem>();
    }
}