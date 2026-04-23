using AutoMapper;
using BusinessLayer.DTOs.Branch;
using BusinessLayer.DTOs.Contact;
using BusinessLayer.DTOs.Teacher;
using BusinessLayer.DTOs.User;
using EntityLayer.Concrete;
using PathojaPilatesProject.Models.Branch;
using PathojaPilatesProject.Models.Contact;
using PathojaPilatesProject.Models.Product;
using PathojaPilatesProject.Models.Teacher;
using PathojaPilatesProject.Models.WebSite.Register;

namespace PathojaPilatesProject.Mapping
{
    public class UiMappingProfile : Profile
    {
        public UiMappingProfile()
        {
            CreateMap<TeacherUpdateDto, TeacherEditVM>().ReverseMap();
            CreateMap<SignUpVM, SignUpDto>().ReverseMap();
            CreateMap<BranchListDto, BranchListVM>();
            CreateMap<LoginVM, LoginDto>();
            CreateMap<ContactPageVM, ContactCreateDto>()
                .ForMember(d => d.Type,o => o.MapFrom(_ => ContactType.GeneralContact))
                .ForMember(d => d.FullName,o => o.MapFrom(s => $"{s.FirstName} {s.LastName}".Trim()))
                .ForMember(d => d.Email,o => o.MapFrom(s => s.Email ?? string.Empty))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone ?? string.Empty))
                .ForMember(d => d.Subject, o => o.MapFrom(_ => "Genel İletişim Mesajı"))
                .ForMember(d => d.Message,o => o.MapFrom(s => s.Message));

            CreateMap<ProductDetailVM, ContactCreateDto>()
                .ForMember(d => d.Type, o => o.MapFrom(_ => ContactType.PackageInfoContact))
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.CategoryName))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName ?? string.Empty))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email ?? string.Empty))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone ?? string.Empty))
                .ForMember(d => d.Subject, o => o.MapFrom(_ => "Paket Bilgilendirme Talebi"))
                .ForMember(d => d.KvkkApproved, o => o.MapFrom(s => s.KvkkApproved));

            CreateMap<TeacherListForWebsiteDto, TeacherCardVM>();
        }
    }
}
