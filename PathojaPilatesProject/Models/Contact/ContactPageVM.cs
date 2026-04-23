using EntityLayer.Concrete;
using PathojaPilatesProject.Models.Branch;

namespace PathojaPilatesProject.Models.Contact
{
    public class ContactPageVM
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Message { get; set; }
        public List<BranchListVM> Branches { get; set; } = new();
        //public IEnumerable<BranchLocation> Branches { get; set; } = Enumerable.Empty<BranchLocation>();
    }
}
