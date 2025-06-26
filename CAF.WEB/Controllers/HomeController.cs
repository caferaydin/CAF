using System.Diagnostics;
using CAF.WEB.Models;
using Kronos.StationModule.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStationService _stationService;


        public HomeController(ILogger<HomeController> logger, IStationService stationService)
        {
            _logger = logger;
            _stationService = stationService;
        }

        public async Task<IActionResult> Index()
        {
            var stations = await _stationService.GetStations();

            FatihModel fatihModel = new FatihModel
            {
                Station = stations,
                Machine = stations
            };  

            return View(fatihModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
