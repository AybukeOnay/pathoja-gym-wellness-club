using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class MemberTeacher
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public bool Active { get; set; } = true;
    }
}
