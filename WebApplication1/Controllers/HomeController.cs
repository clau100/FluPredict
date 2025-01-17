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

    [HttpGet]
    public IActionResult ResultsGood()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResultsBad()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile csvFile)
    {
        if (csvFile == null || csvFile.Length == 0 || !csvFile.FileName.EndsWith(".csv"))
        {
            ModelState.AddModelError("csvFile", "Please select a CSV file.");
            return View();
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", csvFile.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await csvFile.CopyToAsync(stream);
        }

        string pythonScriptPath =
            @"C:\Users\claud\Downloads\projects-oricepunemmergebinev2-main\code\project"; //absolute path, trebuie schimbat la relative path
        string csvPath = filePath; //eventual modificat cu datele efective ale unui pacient

        try
        {
            using (var modelWrapper = new ModelWrapper(csvPath, pythonScriptPath))
            { 
                var predictions = modelWrapper.Predict();
                if (predictions[0] == 1)
                {
                    return RedirectToAction("ResultsGood");
                }
                return RedirectToAction("ResultsBad");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return RedirectToAction("Index");
    }
    
}