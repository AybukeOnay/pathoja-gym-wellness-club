using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Teacher
{
    public class TeacherListDto
    {
        public int Id { get; set; }           // Teacher.Id
        public string FullName { get; set; }      // User.Name + User.LastName
        public string Mail { get; set; }      // User.Mail
        public string? PhoneNumber { get; set; } // User.PhoneNumber
        public string? Image { get; set; }    // Teacher.Image
        public List<string> Categories { get; set; }
        public List<string> Branches { get; set; }
    }
}
