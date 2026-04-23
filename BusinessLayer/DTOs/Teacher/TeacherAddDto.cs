using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Teacher
{
    public class TeacherAddDto
    { 
        // User bilgileri
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }

        // Teacher bilgileri
        public IFormFile ImageFile { get; set; }
        public string? Image { get; set; }

        // İlişkiler
        public List<int> CategoryIds { get; set; } = new();
        public List<int> BranchIds { get; set; } = new();
    }
}
