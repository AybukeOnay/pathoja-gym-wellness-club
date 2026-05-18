using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.Contact;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PathojaPilatesProject.Models.Branch;
using PathojaPilatesProject.Models.Contact;
using QRCoder;

namespace PathojaPilatesProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        private readonly IContactService _contactService;
        private readonly IValidator<ContactPageVM> _contactPageValidator;

        public ContactController(IBranchService branchService,IContactService contactService,IMapper mapper,IValidator<ContactPageVM> contactPageValidator)
        {
            _branchService = branchService;
            _contactService = contactService;
            _mapper = mapper;
            _contactPageValidator = contactPageValidator;
        }

        public async Task<IActionResult> Index()
        {
            var branchList = await _branchService.GetActiveBranchesAsync();
            var vm = new ContactPageVM
            {
                Branches = _mapper.Map<List<BranchListVM>>(branchList)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactPageVM model)
        {
            var validationResult = _contactPageValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();

                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                model.Branches = await GetBranchVMListAsync();

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.Branches = await GetBranchVMListAsync();
                return View(model);
            }

            var dto = new ContactCreateDto
            {
                Type = ContactType.GeneralContact,
                FullName = $"{model.FirstName} {model.LastName}".Trim(),
                Email = model.Email!,
                Phone = model.Phone!,
                Subject = "İletişim Formu",
                Message = model.Message,
                KvkkApproved = true
            };

            await _contactService.CreateContactAsync(dto);

            TempData["ContactSuccess"] = "Talebiniz alındı. En kısa sürede sizinle iletişime geçeceğiz.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<BranchListVM>> GetBranchVMListAsync()
        {
            var branchList = await _branchService.GetActiveBranchesAsync();
            return _mapper.Map<List<BranchListVM>>(branchList);
        }

        [HttpGet]
        public async Task<IActionResult> LocationQr(int branchId)
        {
            var branch = await _branchService.GetByIdAsync(branchId);
            if (branch == null)
                return NotFound();

            // Branch'ten Google Maps linkini al
            var googleMapsUrl = branch.GoogleMapsUrl;

            // (Opsiyonel) Boşsa adres/lat-lng'den kendin oluşturmak istersen burada fallback yazabilirsin

            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(googleMapsUrl, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(qrData);
            byte[] qrBytes = qrCode.GetGraphic(10); // 10: daha kompakt QR

            return File(qrBytes, "image/png");
        }
    }
}
