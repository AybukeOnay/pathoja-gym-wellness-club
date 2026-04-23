namespace PathojaPilatesProject.Models.Category
{
    public class CategoryChipVM
    {
        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Slug { get; set; } = "";
        public string Name { get; set; } = "";
        public bool HasChildren { get; set; }
    }
}
