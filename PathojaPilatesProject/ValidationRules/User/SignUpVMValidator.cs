using BusinessLayer.DTOs.Teacher;
using FluentValidation;
using PathojaPilatesProject.Models.Teacher;
using PathojaPilatesProject.Models.WebSite.Register;

namespace PathojaPilatesProject.ValidationRules.User
{
    public class SignUpVMValidator : AbstractValidator<SignUpVM>
    {
        public SignUpVMValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Ad alanı en az 2 karakter olmalıdır")
                .MaximumLength(40).WithMessage("Ad en fazla 40 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Soyad alanı en az 2 karakter olmalıdır")
                .MaximumLength(40).WithMessage("Soyad en fazla 40 karakter olabilir.");

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("E-posta adresi boş bırakılamaz.")
                 .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                 .Must(BeValidDomain).WithMessage("Geçerli bir mail adresi giriniz. Örneğin deneme@gmail.com");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches(@"\d").WithMessage("Şifre en az bir rakam içermelidir.")
                .Matches(@"[\W_]")
                .WithMessage("Şifre en az bir özel karakter içermelidir.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
                .Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor.");
        }

        private bool BeValidDomain(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            var allowedDomains = new[] { "gmail.com", "hotmail.com", "outlook.com", "yahoo.com" };
            var domain = email.Split('@').LastOrDefault();

            return domain != null && allowedDomains.Contains(domain.ToLower());
        }
    }
}
