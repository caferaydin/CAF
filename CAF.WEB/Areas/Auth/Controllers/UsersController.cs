using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Abstractions.Services.ExternalService;
using CAF.Application.Helpers;
using CAF.Application.Models.Authentication.Request;
using CAF.Application.Models.Authentication.Requests;
using CAF.Domain.Entities.Authentication;
using CAF.WEB.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Areas.Auth.Controllers;

[Area("Auth")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IMailService _mailService;
    private readonly UserManager<AppUser> _userManager;

    public UsersController(IUserService userService, IMailService mailService, UserManager<AppUser> userManager)
    {
        _userService = userService;
        _mailService = mailService;
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserRequest createUserCommandRequest)
    {
        bool isUsernameUnique = await _userService.IsUsernameUniqueAsync(createUserCommandRequest.Username);
        bool isEmailUnique = await _userService.IsEmailUniqueAsync(createUserCommandRequest.Email);
        bool isPhoneNumberUnique = await _userService.IsPhoneNumberUniqueAsync(createUserCommandRequest.PhoneNumber);

        if (!isUsernameUnique)
        {
            ModelState.AddModelError("Username", "Kullanıcı adı zaten kullanımda.");
        }
        if (!isEmailUnique)
        {
            ModelState.AddModelError("Email", "E-posta adresi zaten kullanımda.");
        }
        if (!isPhoneNumberUnique)
        {
            ModelState.AddModelError("PhoneNumber", "Telefon numarası zaten kullanımda.");
        }

        if (ModelState.IsValid)
        {
            // Komutu işleyin
            var response = await _userService.CreateAsync(createUserCommandRequest);

            if (response.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // İşlem başarısızsa, hata mesajını ekleyin
            ModelState.AddModelError(string.Empty, response.Message);
        }

        // Model doğrulama veya işlem başarısızsa kayıt formunu tekrar göster
        return View("Register", createUserCommandRequest);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordCommandRequest)
    {
        await _userService.UpdatePasswordAsync(updatePasswordCommandRequest);

        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var model = await _userManager.GetUserAsync(User);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> checkUsername(string username)
    {
        bool isUnique = await _userService.IsUsernameUniqueAsync(username);
        return Ok(isUnique);
    }

    [HttpGet]
    public async Task<IActionResult> checkEmail(string email)
    {
        bool isUnique = await _userService.IsEmailUniqueAsync(email);
        return Ok(isUnique);
    }

    [HttpGet]
    public async Task<IActionResult> checkPhone(string phone)
    {
        string cleanedPhone = ApplicationHelpers.CleanPhoneNumber(phone);

        bool isUnique = await _userService.IsPhoneNumberUniqueAsync(cleanedPhone);
        return Ok(isUnique);
    }
}
