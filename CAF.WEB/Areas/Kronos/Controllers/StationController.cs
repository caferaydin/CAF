using Kronos.StationModule.Model.Requests;
using Kronos.StationModule.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Areas.Kronos.Controllers;

[Area("Kronos")]
public class StationController : Controller
{
    private readonly IStationService _stationService;

    public StationController(IStationService stationService)
    {
        _stationService = stationService;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetPagedStations([FromBody] StationPagedRequest input)
    {
        var result = await _stationService.GetPagedStationsAsync(input);

        return Ok(new
        {
            data = result.Items,
            recordsTotal = result.TotalCount,
            recordsFiltered = result.TotalCount
        });
    }
}
