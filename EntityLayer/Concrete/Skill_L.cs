using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Skill_L
    {
        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        [Required,MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
