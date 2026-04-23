using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Option_L
    {
        public int OptionId { get; set; }
        public Option Option { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Value { get; set; }
    }
}
