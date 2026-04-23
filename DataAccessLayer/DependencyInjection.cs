using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAboutDal, EfAboutRepository>();
            services.AddScoped<IBranchDal, EfBranchRepository>();
            services.AddScoped<ICampaignDal, EfCampaignRepository>();
            services.AddScoped<ICategoryBranchDal, EfCategoryBranchRepository>();
            services.AddScoped<ICategoryDal, EfCategoryRepository>();
            services.AddScoped<ICategoryTeacherDal, EfCategoryTeacherRepository>();
            services.AddScoped<ICurrencyDal, EfCurrencyRepository>();
            services.AddScoped<ILanguageDal, EfLanguageRepository>();
            services.AddScoped<IMemberDal, EfMemberRepository>();
            services.AddScoped<IMemberTeacherDal, EfMemberTeacherRepository>();
            services.AddScoped<IOptionDal, EfOptionRepository>();
            services.AddScoped<IPaymentDal, EfPaymentRepository>();
            services.AddScoped<IPaymentMethodDal, EfPaymentMethodRepository>();
            services.AddScoped<IPolicyDal, EfPolicyRepository>();
            services.AddScoped<IProductDal, EfProductRepository>();
            services.AddScoped<IProductOptionDal, EfProductOptionRepository>();
            services.AddScoped<IProductPriceDal, EfProductPriceRepository>();
            services.AddScoped<ITeacherDal, EfTeacherRepository>();
            services.AddScoped<IUserDal, EfUserRepository>();
            services.AddScoped<IContactDal, EfContactRepository>();

            return services;
        }
    }
}
