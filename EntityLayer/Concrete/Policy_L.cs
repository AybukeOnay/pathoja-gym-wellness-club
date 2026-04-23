using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Policy_L
    {
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
