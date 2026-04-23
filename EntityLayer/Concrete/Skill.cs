using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Skill
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Slug { get; set; } = string.Empty;

        public bool Active { get; set; } = true;

        public ICollection<Skill_L> Skill_L { get; set; }
        public ICollection<TeacherSkill> TeacherSkills { get; set; }
    }
}
