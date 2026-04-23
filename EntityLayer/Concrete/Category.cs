using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Category
    {
        public int Id { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }

        [Required, MaxLength(120)]
        public string Slug { get; set; } = string.Empty;
        public string? Image { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<Product> Products { get; set; }
        public ICollection<Category_L> Category_L { get; set; }
        public ICollection<CategoryBranch> CategoryBranches { get; set; }
        public ICollection<CategoryTeacher> CategoryTeachers { get; set; }
    }
}
