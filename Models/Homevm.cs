using System.Collections.Generic;

namespace WebOS.Models
{
    public class Homevm
    {
        public IEnumerable<Fatwa> Fatwas { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public List<Hadith> allHadith { get; set; } 
        public Fatwa Fatwa { get; set; }
        public Category Category { get; set; }
    }
}
