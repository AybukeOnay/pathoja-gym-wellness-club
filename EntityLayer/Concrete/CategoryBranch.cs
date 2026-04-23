using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class CategoryBranch
    {
        public int Id { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public bool Active { get; set; } = true;

        public ICollection<TeacherCategoryBranch> TeacherCategoryBranches { get; set; }
    }
}
