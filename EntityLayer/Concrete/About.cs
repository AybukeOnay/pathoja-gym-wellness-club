using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class About
    {
        public int Id { get; set; }
        public ICollection<About_L> About_L { get; set; }
        public bool Active { get; set; } = true;
    }
}
