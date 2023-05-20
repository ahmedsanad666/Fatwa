using System;
using System.ComponentModel.DataAnnotations;

namespace WebOS.Models
{
    public class Fatwa
    {
        [Key]
        [Display(Name = "رقم الفتوى")]
        public int Id { get; set; }

        [Display(Name = "الباب الفقهي")]
        public int CategoryId { get; set; }
        [Display(Name = "الباب الفقهي")]
        public virtual Category Category { get; set; }

        [Display(Name = "المفتي")]
        public int ScholarId { get; set; }
        [Display(Name = "المفتي")]
        public virtual Scholar Scholar { get; set; }

        [StringLength(1000)]
        [Display(Name = "عنوان الفتوى")]
        public string Title { get; set; }


        [Display(Name = "السؤال")]
        public string Question { get; set; }

        [Display(Name = "الجواب")]
        public string Answer { get; set; }

        [StringLength(500)]
        [Display(Name = "الموضوع الفقهي")]
        public string Tags { get; set; }

        [Display(Name = "عدد القراء")]
        public int Reads { get; set; }

        [Display(Name = "التأريخ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime EntryDate { get; set; }

    }
}
