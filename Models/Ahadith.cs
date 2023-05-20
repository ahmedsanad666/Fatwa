using System.Collections.Generic;

namespace WebOS.Models
{
   
        public class Data
        {
            public string name { get; set; }
            public string id { get; set; }
            public int available { get; set; }
            public int requested { get; set; }
            public List<Hadith> hadiths { get; set; }
        }

        public class Hadith
        {
            public int number { get; set; }
            public string arab { get; set; }
            public string id { get; set; }
        }

        public class Root
        {
            public int code { get; set; }
            public string message { get; set; }
            public Data data { get; set; }
            public bool error { get; set; }
        }


    
}
