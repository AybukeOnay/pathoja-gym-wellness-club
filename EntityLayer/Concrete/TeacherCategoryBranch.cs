using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class TeacherCategoryBranch
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int CategoryBranchId { get; set; }
        public CategoryBranch CategoryBranch { get; set; }

        public bool Active { get; set; } = true;
    }
}
