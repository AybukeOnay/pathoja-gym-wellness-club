using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Language
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        [RegularExpression("^[A-Z]{2}$")]
        public string Code { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        public string? FlagIcon { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<About_L> About_L { get; set; }
        public ICollection<Campaign_L> Campaign_L { get; set; }
        public ICollection<Category_L> Category_L { get; set; }
        public ICollection<Option_L> Option_L { get; set; }
        public ICollection<Policy_L> Policy_L { get; set; }
        public ICollection<Product_L> Product_L { get; set; }
        public ICollection<Skill_L> Skill_L { get; set; }

    }
}
