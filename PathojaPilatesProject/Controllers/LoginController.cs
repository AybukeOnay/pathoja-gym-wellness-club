using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PathojaPilatesProject.Models.WebSite.Register;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PathojaPilatesProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LoginController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //Yeni Kayıt
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM model)
        {

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // VM → DTO dönüşümü (UI Mapping)
                var dto = _mapper.Map<SignUpDto>(model);

                // Manager → Entity mapping & eklemeler
                await _userService.AddUserAsync(dto);

                TempData["SuccessMessage"] = "Kayıt işleminiz başarıyla tamamlandı";
                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = _mapper.Map<LoginDto>(model);
            
            var checkUser = await _userService.CheckUserAsync(dto.Email, dto.Password);
            if (!checkUser)
            {
                ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
                return View(model);
            }

            var user = await _userService.GetByEmailAsync(dto.Email);
            if (user == null || !user.Active)
            {
                ModelState.AddModelError(string.Empty, "Hesap pasif veya bulunamadı.");
                return View(model);
            }


            string roleName = user.UserRoleId switch
            {
                1 => "Admin",
                2 => "Teacher",
                _ => "Member"
            };

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Mail),
                    new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}".Trim()),
                    new Claim(ClaimTypes.Role, roleName)
                };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14),
                    AllowRefresh = true
                });

            if (roleName == "Admin")
                return RedirectToAction("Index", "Teacher");

            if (roleName == "Teacher")
                return RedirectToAction("Index", "Profile");

            if (roleName == "Member")
                return RedirectToAction("Index", "Profile");

            //if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            //    return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]                                
        [HttpPost]
        [ValidateAntiForgeryToken]                  
        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session?.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
