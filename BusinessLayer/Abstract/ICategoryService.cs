using BusinessLayer.DTOs.Category;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task<List<Category>> GetCategoriesWithLanguagesAsync();
        Task<List<CategoryListDto>> GetCategoryCardsAsync(int languageId, int? branchId);
        Task<List<CategoryFilterForChipDto>> GetCategoryFiltersAsync(int languageId, int? branchId);
        Task<List<Category>> GetActiveCategoriesForNavbarAsync();
    }
}
