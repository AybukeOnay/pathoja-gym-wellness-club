using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Product_L
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string? Header { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
