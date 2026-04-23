using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Policy
    {
        public int Id { get; set; }
        public bool Active { get; set; } = true;
        public ICollection<Policy_L> Policy_L { get; set; }
    }
}
