using Microsoft.AspNetCore.Mvc;
using Online_Shop.Data;
using Online_Shop.Models;

using Online_Shop.ViewModels;

namespace Online_Shop.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
