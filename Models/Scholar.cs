using System.ComponentModel.DataAnnotations;

namespace WebOS.Models
{
    public class Scholar
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        [Display(Name = "المنصب")]
        public string PositioName { get; set; }

        [StringLength(50)]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Display(Name = "نبذة مختصرة")]
        public string Bio { get; set; }

        [StringLength(100)]
        [Display(Name = "الصورة")]
        public string Image { get; set; }

        [Display(Name = "الترتيب")]
        public int Indx { get; set; }

    }
}
