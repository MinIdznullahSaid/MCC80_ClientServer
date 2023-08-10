using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.Controllers;

public class LatihanController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Index()
    {
        return View();
    }

}