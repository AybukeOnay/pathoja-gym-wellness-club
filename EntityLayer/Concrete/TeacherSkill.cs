using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class TeacherSkill
    {
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        public bool Active { get; set; } = true;
    }
}
