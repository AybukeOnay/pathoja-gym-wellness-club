using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.DTOs.Category;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CategoryManager : GenericManager<Category>, ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly IMapper _mapper;

        public CategoryManager(ICategoryDal categoryDal, IMapper mapper) : base(categoryDal)
        {
            _categoryDal = categoryDal;
            _mapper = mapper;
        }

        public async Task<List<Category>> GetCategoriesWithLanguagesAsync()
        {
            return await _categoryDal.GetCategoriesWithLanguagesAsync();
        }

        public async Task<List<CategoryListDto>> GetCategoryCardsAsync(int languageId, int? branchId)
        {
            languageId = 1; // TODO: Dil dinamik olacak

            var q = _categoryDal.QueryForCategoryCards(); 
            q = q.Where(c => c.Category_L.Any(l => l.LanguageId == languageId));

            if (branchId.HasValue)
            {
                q = q.Where(c => c.CategoryBranches
                                 .Any(cb => cb.BranchId == branchId.Value));
            }

            // 5) DTO projeksiyonu (sadece gereken alanları al) + async materialize
            var list = await q.Select(c => new CategoryListDto
            {
                Id = c.Id,

                Title = c.Category_L
                         .Where(l => l.LanguageId == languageId)
                         .Select(l => l.Name)
                         .FirstOrDefault(),

                Description = c.Category_L
                               .Where(l => l.LanguageId == languageId)
                               .Select(l => l.Description)
                               .FirstOrDefault(),


                ImageUrl = string.IsNullOrWhiteSpace(c.Image)
                            ? "/images/Category/placeholder.jpg"
                            : "/images/Category/" + c.Image,

                // Branch.Name -> slug listesi (UI ile birebir uyum)
                Branches = c.CategoryBranches
                       .Select(cb => cb.Branch.Name)   // ✅ sadece Name
                       .ToList(),

                BranchIds = c.CategoryBranches
                 .Select(cb => cb.BranchId)
                 .ToList(),

                CategorySlug = c.Slug,

                IsWide = false
            }).ToListAsync();

            return list;
        }

        public async Task<List<CategoryFilterForChipDto>> GetCategoryFiltersAsync(int languageId, int? branchId)
        {
            languageId = 1; // TODO: dil dinamik

            var q = _categoryDal.QueryForCategoryCards();

            if (branchId.HasValue)
            {
                q = q.Where(c => c.CategoryBranches
                                 .Any(cb => cb.BranchId == branchId.Value));
            }

            var list = await q
                .AsNoTracking()
                .Where(c => c.Active)
                .Select(c => new CategoryFilterForChipDto
                {
                    Id = c.Id,
                    ParentCategoryId = c.ParentCategoryId,
                    Slug = c.Slug,
                    Name = c.Category_L
                        .Where(l => l.LanguageId == languageId)
                        .Select(l => l.Name)
                        .FirstOrDefault() ?? c.Slug
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .ToListAsync();

            var tr = StringComparer.Create(new CultureInfo("tr-TR"), ignoreCase: true);

            return list
                .OrderBy(x => x.Name, tr)
                .ToList();
        }

        public async Task<List<Category>> GetActiveCategoriesForNavbarAsync()
        {
            return await _categoryDal.GetActiveCategoriesForNavbarAsync();
        }
    }

    
}

