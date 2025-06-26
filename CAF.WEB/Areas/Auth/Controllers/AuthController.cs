using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Abstractions.Services.ExternalService;
using CAF.Application.Models.Authentication.Request;
using CAF.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Areas.Auth.Controllers;

[Area("Auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMailService _mailService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AuthController(IAuthService authService, IMailService mailService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _authService = authService;
        _mailService = mailService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Profile", "Users", new { area = "Auth" });
        }

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var response = await _authService.LoginAsync(model);
        if (response.ResultCode == 0)
        {
            return RedirectToAction("Profile", "Users", new { area = "Auth" });
        }

        ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RefreshTokenLogin(RefreshTokenRequest model)
    {
        var response = await _authService.RefreshTokenLoginAsync(model);
        if (response.ResultCode == 0)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Bir hata oluştu");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> PasswordReset(PasswordResetRequest model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var response = await _authService.PasswordResetAsync(model);
        if (response)
        {
            // Şifre sıfırlama başarılıysa bir mesaj gösterebiliriz.
            ViewBag.Message = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.";
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Kullanıcı Bulunamadı");
        }

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> VerifyResetToken(ResetTokenRequest model)
    {
        var response = await _authService.VerifyResetTokenAsync(model);
        if (response)
        {
            // Token doğrulandıysa şifreyi sıfırlamak için kullanıcıyı yönlendirebiliriz.
            return RedirectToAction("ResetPassword");
        }

        ModelState.AddModelError(string.Empty, "Hatalı");
        return View(model);
    }

    [HttpGet]
    public IActionResult PasswordReset()
    {
        return View();
    }

    [HttpGet]
    public IActionResult VerifyResetToken()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        // Kullanıcının oturumunu kapat
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        // Cookie'yi sil
        Response.Cookies.Delete(".AspNetCore.Identity.Application");

        // Giriş sayfasına veya anasayfaya yönlendir
        return RedirectToAction("Login", "Auth");
    }
}
