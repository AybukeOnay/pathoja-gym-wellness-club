using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Teacher
{
    public class TeacherListForWebsiteDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string? Title { get; set; }              // Pilates Instructor
        public string? Area { get; set; }               // Reformer • Mat Pilates
        public string? BranchLabel { get; set; }        // İncek / Alacaatlı
        public string? ImageUrl { get; set; }
        public string? Bio { get; set; }

        // İleride istersen açarsın
        //public List<string> Tags { get; set; } = new();
    }
}
