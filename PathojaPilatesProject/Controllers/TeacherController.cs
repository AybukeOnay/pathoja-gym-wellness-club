using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.Teacher;
using BusinessLayer.ValidationRules.Teacher;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PathojaPilatesProject.Models.Teacher;
using System.Globalization;

namespace PathojaPilatesProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly ICategoryService _categoryService;
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        public TeacherController(ITeacherService teacherService, ICategoryService categoryService, IBranchService branchService,IMapper mapper)
        {
            _teacherService = teacherService;
            _categoryService = categoryService;
            _branchService = branchService;
            _mapper = mapper;
        }

        //ADMIN
        public async Task<IActionResult> Index()
        {
            var teacherList = await _teacherService.GetActiveTeachersWithUserAndCategory();
            return View(teacherList);
        }

        [HttpGet]
        public async Task<IActionResult> AddTeacher()
        {
            var categories = await _categoryService.GetCategoriesWithLanguagesAsync();
            var branches = await _branchService.GetAllAsync();
            
            var vm = new TeacherAddVM
            {
                Teacher = new TeacherAddDto(),
                CategoryList = categories
                .Select(x => new SelectListItem
                {
                    Text = x.Category_L.FirstOrDefault(l => l.Language != null && l.Language.Id == 2)?.Name ?? "Kategori Adı Yok",
                    Value = x.Id.ToString()
                }).ToList(),
                BranchList = branches
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
        };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher(TeacherAddVM vm)
        {

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetCategoriesWithLanguagesAsync();
                var branches = await _branchService.GetAllAsync();

                vm.CategoryList = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Category_L.FirstOrDefault(l => l.LanguageId == 2)?.Name ?? "Kategori Adı Yok"
                }).ToList();

                vm.BranchList = branches.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                }).ToList();

                return View(vm);

            }

            if(vm.Teacher.ImageFile != null && vm.Teacher.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.Teacher.ImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/teachers", fileName);

                // Klasör yoksa oluştur
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await vm.Teacher.ImageFile.CopyToAsync(stream);
                }

                vm.Teacher.Image = "/images/teachers/" + fileName; // DB’ye kaydedilecek path
            }

            await _teacherService.AddTeacherAsync(vm.Teacher);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditTeacher(int id)
        {
            var teacherDto = await _teacherService.GetTeacherForUpdateAsync(id);
            if (teacherDto == null) return NotFound();

            var categories = await _categoryService.GetCategoriesWithLanguagesAsync();
            var branches = await _branchService.GetAllAsync();

            //TO-DO DİL DİNAMİK OLMALI
            ViewBag.categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Category_L.FirstOrDefault(l => l.Language != null && l.Language.Id == 2)?.Name ?? "Kategori Adı Yok",
                Selected = teacherDto.CategoryIds.Contains(c.Id)
            }).ToList();

            ViewBag.branches = branches.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name,
                Selected = teacherDto.BranchIds.Contains(b.Id)
            }).ToList();

            var vm = _mapper.Map<TeacherEditVM>(teacherDto);

            return PartialView("_PartialEditTeacher", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeacher(TeacherEditVM vm)
        {
            if (!ModelState.IsValid)
                return PartialView("_PartialEditTeacher", vm);

            var dto = _mapper.Map<TeacherUpdateDto>(vm);

            try
            {
                await _teacherService.UpdateTeacherAsync(dto);
                return Json(new { success = true });
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_PartialEditTeacher", vm);
            }
        }

        //WEBSITE
        [HttpGet]
        public async Task<IActionResult> ListTeacher()
        {
            var teacherDtos = await _teacherService.GetTeacherListForWebsiteAsync();
            var model = _mapper.Map<List<TeacherCardVM>>(teacherDtos);

            var tr = StringComparer.Create(new CultureInfo("tr-TR"), ignoreCase: true);

            var priorityNames = new HashSet<string>(new[] { "Furkan KURT", "Özge Nur ÖZKAYA" }, tr);

            var pinned = model
                .Where(x => priorityNames.Contains(x.FullName))
                .OrderBy(x => x.FullName, tr) // kendi aralarında sıralansın
                .ToList();

            var others = model
                .Where(x => !priorityNames.Contains(x.FullName))
                .OrderBy(x => x.FullName, tr)
                .ToList();

            model = pinned.Concat(others).ToList();

            return View(model);
        }

    }
}
