using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.Category;
using BusinessLayer.DTOs.Contact;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using PathojaPilatesProject.Models.Category;
using PathojaPilatesProject.Models.Product;

namespace PathojaPilatesProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IContactService _contactService;
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService,
            IMapper mapper,
            IProductService productService,
            IContactService contactService,
            IBranchService branchService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _productService = productService;
            _contactService = contactService;
            _branchService = branchService;
        }
        public async Task<IActionResult> Index(int? branchId, string cat = "all")
        {
            const int languageId = 1;

            List<CategoryListDto> dtos;

            if (!string.Equals(cat, "all", StringComparison.OrdinalIgnoreCase))
            {
                // Önce branch bağımsız tüm kayıtları çek
                var allDtos = await _categoryService.GetCategoryCardsAsync(languageId, null);

                // Seçilen kategoriye ait tüm kayıtlar
                var categoryDtos = allDtos
                    .Where(x => string.Equals(x.CategorySlug, cat, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // Bu kategori hangi branch'lerde var?
                var categoryBranchNames = categoryDtos
                    .SelectMany(x => x.Branches ?? new List<string>())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var distinctBranchIds = categoryDtos
                    .SelectMany(x => x.BranchIds)
                    .Distinct()
                    .ToList();

                if (distinctBranchIds.Count == 1)
                {
                    branchId = distinctBranchIds.First();
                }

                // Aktif branch listesi
                var branches = await _branchService.GetActiveBranchesAsync();

                // Branch name -> id eşleştirme
                var matchingBranchIds = branches
                    .Where(b => categoryBranchNames.Any(cb =>
                        string.Equals(cb, b.Name, StringComparison.OrdinalIgnoreCase)))
                    .Select(b => b.Id)
                    .Distinct()
                    .ToList();

                // Eğer kategori sadece tek şubede varsa branch otomatik o şubeye geçsin
                if (matchingBranchIds.Count == 1)
                {
                    branchId = matchingBranchIds.First();
                }

                // Son listeyi branch durumuna göre oluştur
                if (branchId.HasValue)
                {
                    var selectedBranch = branches.FirstOrDefault(b => b.Id == branchId.Value);

                    dtos = categoryDtos
                        .Where(x => x.Branches != null &&
                                    selectedBranch != null &&
                                    x.Branches.Any(br => string.Equals(br, selectedBranch.Name, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }
                else
                {
                    dtos = categoryDtos;
                }

                ViewBag.Branches = branches;
            }
            else
            {
                dtos = await _categoryService.GetCategoryCardsAsync(languageId, branchId);
                ViewBag.Branches = await _branchService.GetActiveBranchesAsync();
            }

            var model = dtos.Select(d => new CategoryCardVM
            {
                Id = d.Id,
                CategorySlug = d.CategorySlug ?? string.Empty,
                Title = d.Title,
                Description = d.Description,
                ImageUrl = d.ImageUrl,
                BranchCodes = d.Branches ?? new List<string>(),
                IsWide = d.IsWide
            }).ToList();

            var filterDtos = await _categoryService.GetCategoryFiltersAsync(languageId, branchId);

            var allChips = filterDtos.Select(f => new CategoryChipVM
            {
                Id = f.Id,
                ParentCategoryId = f.ParentCategoryId,
                Slug = f.Slug,
                Name = f.Name
            }).ToList();

            var parentChips = allChips
                .Where(x => x.ParentCategoryId == null)
                .OrderBy(x => x.Name)
                .Select(x => new CategoryChipVM
                {
                    Id = x.Id,
                    ParentCategoryId = x.ParentCategoryId,
                    Slug = x.Slug,
                    Name = x.Name,
                    HasChildren = allChips.Any(c => c.ParentCategoryId == x.Id)
                })
                .ToList();

            var selectedParentSlug = "all";
            var selectedParentName = string.Empty;
            var visibleChildChips = new List<CategoryChipVM>();

            if (!string.Equals(cat, "all", StringComparison.OrdinalIgnoreCase))
            {
                var selectedChip = allChips
                    .FirstOrDefault(x => string.Equals(x.Slug, cat, StringComparison.OrdinalIgnoreCase));

                if (selectedChip != null)
                {
                    if (selectedChip.ParentCategoryId == null)
                    {
                        selectedParentSlug = selectedChip.Slug;
                        selectedParentName = selectedChip.Name;

                        visibleChildChips = allChips
                            .Where(x => x.ParentCategoryId == selectedChip.Id)
                            .OrderBy(x => x.Name)
                            .ToList();
                    }
                    else
                    {
                        var parentChip = allChips
                            .FirstOrDefault(x => x.Id == selectedChip.ParentCategoryId);

                        if (parentChip != null)
                        {
                            selectedParentSlug = parentChip.Slug;
                            selectedParentName = parentChip.Name;

                            visibleChildChips = allChips
                                .Where(x => x.ParentCategoryId == parentChip.Id)
                                .OrderBy(x => x.Name)
                                .ToList();
                        }
                    }
                }
            }

            ViewBag.SelectedBranchId = branchId;
            ViewBag.SelectedCategory = cat;
            ViewBag.ParentFilters = parentChips;
            ViewBag.VisibleChildFilters = visibleChildChips;
            ViewBag.SelectedParentSlug = selectedParentSlug;
            ViewBag.SelectedParentName = selectedParentName;

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProductsForCategory(int categoryId)
        {
            // örn: HttpContext'ten currentLanguageId çekebilirsin
            int languageId = 1;  //TO-DO DİL DİNAMİK OLMALI

            var dtos = await _productService.GetProductsForCategoryAsync(categoryId, languageId);

            return Json(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> RequestProductLead([FromBody] ProductDetailVM vm)
        {
            if (string.IsNullOrWhiteSpace(vm.FullName) ||
                string.IsNullOrWhiteSpace(vm.Phone) ||
                string.IsNullOrWhiteSpace(vm.Email))
            {
                return BadRequest("Tam adınız, telefon ve e-posta adresiniz zorunludur.");
            }

            if (!vm.KvkkApproved)
            {
                return BadRequest("KVKK onayı zorunludur.");
            }

            var dto = _mapper.Map<ContactCreateDto>(vm);
            dto.Type = ContactType.PackageInfoContact;

            await _contactService.CreateContactAsync(dto);

            return Ok(new { success = true });
        }
    }
}
