using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class About_L
    {
        public int AboutId { get; set; }
        public About About { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public string Title { get; set; }
        public string? ContentText1 { get; set; }
        public string? ContentText2 { get; set; }
        public string? ContentText3 { get; set; }
    }
}
