using System.ComponentModel.DataAnnotations;

namespace WebOS.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name = "الباب الفقهي")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "الصورة")]
        public string Image { get; set; }
    }
}
