using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DosyaYonetimPortali.Models.Entities
{
    public class FileItem
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required, StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(50)]
        public string FileExtension { get; set; } = string.Empty;

        public long FileSize { get; set; } // Byte cinsinden

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        // Dosyanın bulunduğu klasör
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;

        // Dosyayı yükleyen kullanıcı
        public int AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; } = null!;
    }
}