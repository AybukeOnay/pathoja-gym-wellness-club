using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Option
    {
        public int Id { get; set; }
        public bool Active { get; set; } = true;
        public ICollection<Option_L> Option_L { get; set; }
    }
}
