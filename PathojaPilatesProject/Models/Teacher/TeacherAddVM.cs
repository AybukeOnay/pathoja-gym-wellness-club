using BusinessLayer.DTOs.Teacher;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PathojaPilatesProject.Models.Teacher
{
    public class TeacherAddVM
    {
        public TeacherAddDto Teacher { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> BranchList { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
