using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Teacher
{
    public class TeacherUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> BranchIds { get; set; }
    }
}
