using BusinessLayer.DTOs.Teacher;
using FluentValidation;
using PathojaPilatesProject.Models.Teacher;

namespace PathojaPilatesProject.ValidationRules.Teacher
{
    public class TeacherUpdateVMValidator : AbstractValidator<TeacherEditVM>
    {
        public TeacherUpdateVMValidator(IValidator<TeacherUpdateDto> teacherValidator)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim boş olamaz")
                .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalıdır")
                .MaximumLength(30).WithMessage("İsim en fazla 30 karakter olmalıdır");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad boş olamaz")
                .MinimumLength(2).WithMessage("Soyad en az 2 karakter olmalıdır")
                .MaximumLength(30).WithMessage("Soyad en fazla 30 karakter olmalıdır");

            RuleFor(x => x.Mail)
                .NotEmpty().WithMessage("E-mail boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir e-mail adresi giriniz")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("E-mail adresi tam olmalıdır");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.CategoryIds)
                .Must(x => x?.Any() == true)
                .WithMessage("En az bir kategori seçilmelidir");

            RuleFor(x => x.BranchIds)
                .Must(x => x?.Any() == true)
                .WithMessage("En az bir şube seçilmelidir");
        }
    }
}


