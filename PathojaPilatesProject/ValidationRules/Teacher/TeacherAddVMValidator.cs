using BusinessLayer.DTOs.Teacher;
using FluentValidation;
using PathojaPilatesProject.Models.Teacher;

namespace PathojaPilatesProject.ValidationRules.Teacher
{
    public class TeacherAddVMValidator : AbstractValidator<TeacherAddVM>
    {
        public TeacherAddVMValidator(IValidator<TeacherAddDto> teacherValidator)
        {
            RuleFor(x => x.Teacher).SetValidator(teacherValidator);
        }
    }
}
