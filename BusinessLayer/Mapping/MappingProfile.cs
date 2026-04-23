using AutoMapper;
using BusinessLayer.DTOs.Branch;
using BusinessLayer.DTOs.Contact;
using BusinessLayer.DTOs.Product;
using BusinessLayer.DTOs.Teacher;
using BusinessLayer.DTOs.User;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Teacher, TeacherListDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.LastName))
                .ForMember(dest => dest.Mail, opt => opt.MapFrom(src => src.User.Mail))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .AfterMap((src, dest) =>
                {
                    dest.Categories = src.CategoryTeachers != null
                                    ? src.CategoryTeachers
                                        .Where(ct => ct.Category != null && ct.Category.Category_L != null)
                                        .Select(ct => ct.Category.Category_L
                                        .FirstOrDefault(l => l.LanguageId == 1)?.Name) //TO - DO DİL DİNAMİK OLACAK
                                        .Where(name => !string.IsNullOrEmpty(name))
                                        .ToList()
                                    : new List<string>();

                    dest.Branches = src.TeacherBranches != null
                                    ? src.TeacherBranches
                                    .Where(tb => tb.Branch != null)
                                    .Select(tb => tb.Branch.Name)
                                    .Where(name => !string.IsNullOrEmpty(name))
                                    .ToList()
                                : new List<string>();

                });

            CreateMap<Teacher, TeacherUpdateDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Mail, opt => opt.MapFrom(src => src.User.Mail))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.CategoryTeachers.Select(ct => ct.CategoryId)))
                .ForMember(dest => dest.BranchIds, opt => opt.MapFrom(src => src.TeacherBranches.Select(tb => tb.BranchId)));

            CreateMap<SignUpDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Mail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UserCreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.UserRoleId, opt => opt.MapFrom(src => 2));

            CreateMap<Branch, BranchListDto>();

            CreateMap<Product, ProductInfoDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product_L.FirstOrDefault().Name))
                .ForMember(dest => dest.Header, opt => opt.MapFrom(src => src.Product_L.FirstOrDefault().Header))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product_L.FirstOrDefault().Description))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.ProductFeatures))
                .ForMember(dest => dest.SubProducts, opt => opt.MapFrom(src => src.SubProducts)); 
            CreateMap<ProductFeature, ProductFeatureDto>();

            //NOT : EĞER PROPERTY İSİMLERİ BİREBİR AYNIYSA TEKRAR YAZILMASINA GEREK YOK
            CreateMap<ContactCreateDto, Contact>()
                .ForMember(d => d.CreatedDate, o => o.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.Type, o => o.MapFrom(_ => ContactType.PackageInfoContact))
                .ForMember(d => d.Message, o => o.MapFrom(s => s.Message));

            CreateMap<Teacher, TeacherListForWebsiteDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.LastName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image))
                .AfterMap((src, dest) =>
                {
                    var skillNames = src.TeacherSkills != null
                        ? src.TeacherSkills
                            .Where(ts => ts.Active && ts.Skill != null && ts.Skill.Skill_L != null)
                            .Select(ts => ts.Skill.Skill_L
                                .FirstOrDefault(l => l.LanguageId == 1/* TO-DO dil dinamik */)?.Name)
                            .Where(name => !string.IsNullOrWhiteSpace(name))
                            .Distinct()
                            .ToList()
                        : new List<string>();

                    var branchNames = src.TeacherBranches != null
                        ? src.TeacherBranches
                            .Where(tb => tb.Branch != null && !string.IsNullOrWhiteSpace(tb.Branch.Name))
                            .Select(tb => tb.Branch.Name)
                            .Distinct()
                            .ToList()
                        : new List<string>();

                    dest.Area = skillNames.Any() ? string.Join(" • ", skillNames) : "Pilates & Wellness";
                    var shortBranchLabels = new List<string>();

                    if (branchNames.Any())
                    {
                        foreach (var branchName in branchNames.Where(x => !string.IsNullOrWhiteSpace(x)))
                        {
                            var b = branchName.Trim();

                            if (b.Contains("Wellness", StringComparison.OrdinalIgnoreCase))
                                shortBranchLabels.Add("Wellness");

                            if (b.Contains("İncek", StringComparison.OrdinalIgnoreCase) ||
                                b.Contains("Incek", StringComparison.OrdinalIgnoreCase))
                                shortBranchLabels.Add("İncek");

                            if (b.Contains("Alacaatlı", StringComparison.OrdinalIgnoreCase) ||
                                b.Contains("Alacaatli", StringComparison.OrdinalIgnoreCase))
                                shortBranchLabels.Add("Alacaatlı");
                        }
                    }

                    dest.BranchLabel = shortBranchLabels.Any()
                        ? string.Join(" • ", shortBranchLabels.Distinct())
                        : "Pathoja Pilates";

                    if (string.IsNullOrWhiteSpace(dest.FullName))
                        dest.FullName = "Eğitmen";

                    if (string.IsNullOrWhiteSpace(dest.ImageUrl))
                        dest.ImageUrl = "/images/teachers/default-teacher.png";

                    // Title/Bio burada fallback verilebilir (entity alanına göre)
                    if (string.IsNullOrWhiteSpace(dest.Title))
                        dest.Title = "Eğitmen";

                    if (string.IsNullOrWhiteSpace(dest.Bio))
                        dest.Bio = "Uzmanlık alanları doğrultusunda kişiye özel program planlaması yapar.";
                });
        }
    }
}
