using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Areas.Kronos.ViewComponents;

[ViewComponent(Name = "Stations")]
public class StationComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("/Areas/Kronos/Views/Components/Stations/GetStations.cshtml");
    }
}
