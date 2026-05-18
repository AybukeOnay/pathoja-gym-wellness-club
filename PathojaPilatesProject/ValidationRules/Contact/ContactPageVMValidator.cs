using FluentValidation;
using PathojaPilatesProject.Models.Contact;

namespace PathojaPilatesProject.ValidationRules.Contact
{
    public class ContactPageVMValidator : AbstractValidator<ContactPageVM>
    {
        public ContactPageVMValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad alanı zorunludur.")
                .MinimumLength(2).WithMessage("Ad en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad alanı zorunludur.")
                .MinimumLength(2).WithMessage("Soyad en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Mail alanı zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir mail adresi giriniz.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
                .MaximumLength(100).WithMessage("E-posta adresi en fazla 100 karakter olabilir.")
                .WithMessage("Mail adresi ornek@mail.com formatında olmalıdır.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon alanı zorunludur.")
                .Matches(@"^5\d{9}$").WithMessage("Telefon numarası 5 ile başlamalı ve 10 rakam olmalıdır.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Mesaj alanı zorunludur.")
                .MinimumLength(10).WithMessage("Mesajınız en az 10 karakter olmalıdır.")
                .MaximumLength(1000).WithMessage("Mesajınız en fazla 1000 karakter olabilir.");
        }
    }
}

