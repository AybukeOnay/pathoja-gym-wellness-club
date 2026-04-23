using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Teacher
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string? Image { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<MemberTeacher> MemberTeachers { get; set; }
        public ICollection<CategoryTeacher> CategoryTeachers { get; set; }
        public ICollection<TeacherBranch> TeacherBranches { get; set; }
        public ICollection<TeacherCategoryBranch> TeacherCategoryBranches { get; set; }        
        public ICollection<TeacherCertification> TeacherCertifications { get; set; }
        public ICollection<TeacherSkill> TeacherSkills { get; set; }
    }
}
