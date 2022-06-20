using Microsoft.AspNetCore.Mvc;

namespace MVCInventarios.Controllers;
public class AccesoDenegadoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
