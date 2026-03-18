using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PostManagementApp.Models;
using PostManagementApp.Services;
using System.Threading.Tasks;

namespace PostManagementApp.Controllers;

public class HomeController : Controller
{
    private readonly IGraveService _graveService;
    private readonly IDeceasedPersonService _deceasedService;
    private readonly IRelativeService _relativeService;

    public HomeController(
        IGraveService graveService,
        IDeceasedPersonService deceasedService,
        IRelativeService relativeService)
    {
        _graveService = graveService;
        _deceasedService = deceasedService;
        _relativeService = relativeService;
    }

    public async Task<IActionResult> Index()
    {
ViewData["Title"] = "Hệ thống Quản lý Nghĩa trang";

        // Get statistics for dashboard
        var graves = await _graveService.GetAllGravesAsync();
        var deceased = await _deceasedService.GetAllDeceasedAsync();
        var relatives = await _relativeService.GetAllRelativesAsync();

        ViewBag.TotalGraves = graves?.Count() ?? 0;
        ViewBag.TotalDeceased = deceased?.Count() ?? 0;
        ViewBag.TotalRelatives = relatives?.Count() ?? 0;
        ViewBag.OccupiedGraves = graves?.Count(g => g.Status == "Occupied") ?? 0;

        return View();
    }

    public async Task<IActionResult> Database()
    {
ViewData["Title"] = "Cơ sở dữ liệu";

            var graves = await _graveService.GetAllGravesAsync();
            var deceased = await _deceasedService.GetAllDeceasedAsync();
            var relatives = await _relativeService.GetAllRelativesAsync();

            var model = new DatabaseViewModel
            {
                Graves = graves,
                DeceasedPersons = deceased,
                Relatives = relatives
            };

            return View(model);
        }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
