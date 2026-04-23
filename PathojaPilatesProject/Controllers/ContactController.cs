using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
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

        public ContactController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
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
