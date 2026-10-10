using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Error() => View();
}