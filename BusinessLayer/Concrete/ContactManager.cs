using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.Contact;
using BusinessLayer.Utilities.Mail;
using CoreLayer.Utilities.Mail;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;


namespace BusinessLayer.Concrete
{
    public class ContactManager : GenericManager<Contact>, IContactService
    {
        private readonly IContactDal _contactDal;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public ContactManager(IContactDal contactDal, IMapper mapper,IMailService mailService) : base(contactDal)
        {
            _contactDal = contactDal;
            _mapper = mapper;
            _mailService = mailService;
        }

        public async Task CreateContactAsync(ContactCreateDto contactCreateDto)
        {
            var entity = _mapper.Map<Contact>(contactCreateDto);
            await _contactDal.InsertAsync(entity);

            var (subject, body) = MailTemplateService.BuildAdminMail(
                contactCreateDto,
                contactCreateDto.CategoryName);

            await _mailService.SendMailAsync(new MailRequest
            {
                To = "contact@pathoja.com",
                Subject = subject,
                Body = body,
                IsHtml = true
            });

            if (!string.IsNullOrWhiteSpace(contactCreateDto.Email))
            {
                var (userSubject, userBody) = MailTemplateService.BuildUserMail(
                    contactCreateDto,
                    contactCreateDto.CategoryName);

                await _mailService.SendMailAsync(new MailRequest
                {
                    To = contactCreateDto.Email,
                    Subject = userSubject,
                    Body = userBody,
                    IsHtml = true
                });
            }
        }

        //public async Task CreateContactAsync(ContactCreateDto contactCreateDto)
        //{
        //    var entity = _mapper.Map<Contact>(contactCreateDto);
        //    await _contactDal.InsertAsync(entity);

        //    var (subject, body) = MailTemplateService.BuildAdminMail(contactCreateDto);

        //    await _mailService.SendMailAsync(new MailRequest
        //    {
        //        To = "contact@pathoja.com",
        //        Subject = subject,
        //        Body = body,
        //        IsHtml = true
        //    });

        //    var (userSubject, userBody) = MailTemplateService.BuildUserMail(contactCreateDto);
        //    if (!string.IsNullOrWhiteSpace(contactCreateDto.Email))
        //    {
        //        await _mailService.SendMailAsync(new MailRequest
        //        {
        //            To = contactCreateDto.Email,
        //            Subject = userSubject,
        //            Body = userBody,
        //            IsHtml = true
        //        });
        //    }
        //}
    }
}
