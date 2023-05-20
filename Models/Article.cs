using System;
using System.ComponentModel.DataAnnotations;

namespace WebOS.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        [Display(Name = "العنوان")]
        public string Title { get; set; }

        [StringLength(4000)]
        [Display(Name = "الوصف المختصر")]
        public string Description { get; set; }

        [Display(Name = "المحتوى")]
        public string Content { get; set; }

        [StringLength(500)]
        [Display(Name = "الرابط")]
        public string Url { get; set; }

        [StringLength(100)]
        [Display(Name = "الصورة")]
        public string Image { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "تاريخ النشر")]
        public DateTime PublishedAt { get; set; }

        [StringLength(500)]
        [Display(Name = "المصدر")]
        public string Source { get; set; }
    }
}
