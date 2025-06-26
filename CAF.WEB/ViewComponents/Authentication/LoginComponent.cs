using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.ViewComponents.Authentication;

[ViewComponent(Name = "Login")]
public class LoginComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}