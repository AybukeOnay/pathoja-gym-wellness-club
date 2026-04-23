namespace PathojaPilatesProject.Models.Category
{
    public class CategoryCardVM
    {
        public int Id { get; set; }
        public string CategorySlug { get; set; } 
        public string? Title { get; set; } 
        public string? Description { get; set; }
        public string? ImageUrl { get; set; } 
        public List<string> BranchCodes { get; set; } = new List<string>();
        public bool IsWide { get; set; } = false;
    }
}
