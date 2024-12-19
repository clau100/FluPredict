using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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
    
    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile csvFile)
    {
        if (csvFile == null || csvFile.Length == 0)
        {
            ModelState.AddModelError("csvFile", "Please select a CSV file.");
            return View();
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", csvFile.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await csvFile.CopyToAsync(stream);
        }

        // Process the CSV file as needed

        return RedirectToAction("Index");
    }
    
}