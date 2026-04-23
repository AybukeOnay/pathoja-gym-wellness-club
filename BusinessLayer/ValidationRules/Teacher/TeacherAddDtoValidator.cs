using BusinessLayer.DTOs.Teacher;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules.Teacher
{
    public class TeacherAddDtoValidator : AbstractValidator<TeacherAddDto>
    {
        public TeacherAddDtoValidator()
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

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Şifre boş olamaz.")
               .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
               .Matches("[A-Z]").WithMessage("Şifre en az 1 büyük harf içermelidir.")
               .Matches("[a-z]").WithMessage("Şifre en az 1 küçük harf içermelidir.")
               .Matches("[0-9]").WithMessage("Şifre en az 1 rakam içermelidir.")
               .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az 1 özel karakter içermelidir.");

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
