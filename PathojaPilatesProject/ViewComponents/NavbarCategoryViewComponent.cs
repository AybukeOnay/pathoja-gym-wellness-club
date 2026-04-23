using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PathojaPilatesProject.Models.Category;
using PathojaPilatesProject.Models.WebSite.Navbar;

namespace PathojaPilatesProject.ViewComponents
{
    public class NavbarCategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _cache;

        public NavbarCategoryViewComponent(ICategoryService categoryService, IMemoryCache cache)
        {
            _categoryService = categoryService;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            const string cacheKey = "navbar_categories_tr";

            if (!_cache.TryGetValue(cacheKey, out List<NavbarCategoryVM>? model))
            {
                // Servisten kategorileri çek (sende hangi metot varsa onu kullan)
                var categories = await _categoryService.GetActiveCategoriesForNavbarAsync();
                const int langId = 1;

                model = categories
                    .Where(x => x.Active && x.ParentCategoryId == null)
                    .OrderBy(x => x.Id) // istersen sort alanına göre değiştiririz
                    .Select(parent => new NavbarCategoryVM
                    {
                        Id = parent.Id,
                        Name = parent.Category_L?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? parent.Slug,
                        Slug = parent.Slug ?? string.Empty,
                        Children = categories
                            .Where(x => x.Active && x.ParentCategoryId == parent.Id)
                            .OrderBy(x => x.Id)
                            .Select(child => new NavbarCategoryVM
                            {
                                Id = child.Id,
                                Name = child.Category_L?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? child.Slug,
                                Slug = child.Slug ?? string.Empty
                            })
                            .ToList()
                    })
                    .ToList();

                _cache.Set(cacheKey, model, TimeSpan.FromMinutes(30));
            }

            return View(model);
        }
    }
}
