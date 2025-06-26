using CAF.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.ViewComponents.ManagerLayout;

[ViewComponent(Name = "Header")]
public class HeaderComponent : ViewComponent
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public HeaderComponent(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = _signInManager.Context.User;

        if (user == null)
        {
            return View();
        }
        var model = await _userManager.GetUserAsync(user);

        return View(model);
    }
}
