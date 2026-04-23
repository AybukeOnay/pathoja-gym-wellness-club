using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Category
{
    public class CategoryFilterForChipDto
    {

        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Slug { get; set; } = ""; // URL için stabil anahtar (şimdilik Name'den türetiyoruz)
        public string Name { get; set; } = "";
    }
}
