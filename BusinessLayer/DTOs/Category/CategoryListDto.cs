using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Category
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public string CategorySlug { get; set; } = string.Empty; // pilates, hamile, klinik...
        public string? Title { get; set; } = string.Empty;        // Category_L.Name
        public string? Description { get; set; }                 // Category_L.Description
        public string? ImageUrl { get; set; } = string.Empty;     // /images/Hero_Section/...
        public List<string> Branches { get; set; } = new List<string>(); // ["incek","cukurambar"]
        public List<int> BranchIds { get; set; } = new();
        public bool IsWide { get; set; } = false;
    }
}
