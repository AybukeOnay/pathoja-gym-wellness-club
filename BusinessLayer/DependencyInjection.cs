using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using CoreLayer.Utilities.Mail;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayerDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITeacherService, TeacherManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IBranchService, BranchManager>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IContactService, ContactManager>();

            //services.AddScoped<ICategoryService, CategoryManager>();
            //services.AddScoped<ICampaignService, CampaignManager>();
            //services.AddScoped<IMemberService, MemberManager>();
            // Diğer servisleri burada sırayla ekle...

            return services;
        }
    }
}
