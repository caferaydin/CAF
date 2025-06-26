using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.ViewComponents.Authentication;

[ViewComponent(Name = "Register")]
public class RegisterComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}