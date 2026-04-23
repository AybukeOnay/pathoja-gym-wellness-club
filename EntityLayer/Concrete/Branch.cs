using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Branch
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required,MaxLength(250)]
        public string Address { get; set; }

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? GoogleMapsUrl { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<CategoryBranch> CategoryBranches { get; set; }
        public ICollection<CategoryTeacher> CategoryTeachers { get; set; }
        public ICollection<TeacherBranch> TeacherBranches { get; set; }
    }
}
