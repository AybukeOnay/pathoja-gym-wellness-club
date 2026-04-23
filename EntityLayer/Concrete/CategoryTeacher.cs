using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class CategoryTeacher
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public bool Active { get; set; } = true;
    }
}
