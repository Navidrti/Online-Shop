using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Data;
using Online_Shop.Models;
using Online_Shop.Service;
using Online_Shop.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace Online_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Index([FromServices] DbWebFinal db)
        {
            ViewData["Products"] = db.products.ToList();
            ViewData["Descending"] = db.products.OrderByDescending(x => x.id).ToList();
            ViewData["Category"] = db.categories.ToList();
            return View();
        }
        public IActionResult Shop([FromServices] DbWebFinal db, int page = 1, int pageSize = 9, string sortBy = "all")
        {
            var products = db.products.AsQueryable();

            // Sorting functionality
            switch (sortBy)
            {
                case "cheapest":
                    products = products.OrderBy(p => p.Price); // Sort by price (cheapest)
                    break;
                case "mostExpensive":
                    products = products.OrderByDescending(p => p.Price); // Sort by price (most expensive)
                    break;
                default:
                    products = products.OrderBy(p => p.Name); // Default sorting (All)
                    break;
            }

            // Pagination
            var totalProducts = products.Count();
            var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Passing data to the view
            ViewData["Products"] = pagedProducts;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewData["Category"] = db.categories.ToList();
            ViewData["SortBy"] = sortBy; // Keep the selected sorting option

            return View();
        }

        public IActionResult Contact([FromServices] DbWebFinal db)
        {
            ViewData["Category"] = db.categories.ToList();
            return View();
        }
        public IActionResult CheckOut([FromServices] DbWebFinal db)
        {
            var userId = userService.GetUserId();



            ViewData["Cart"] = db.cart.Where(x => x.Userid == userId).ToList();

            ViewData["sub"] = db.cart.Where(x => x.Userid == userId).Sum(x => x.totalPrice);
            ViewData["total"] = db.cart.Where(x => x.Userid == userId).Sum(x => x.totalPrice) + 10;
            ViewData["Category"] = db.categories.ToList();


            return View();
        }

        public IActionResult SignUp([FromServices] DbWebFinal db)
        {
            ViewData["Category"] = db.categories.ToList();
            return View();
        }
        public IActionResult SignIn([FromServices] DbWebFinal db)
        {
            ViewData["Category"] = db.categories.ToList();
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult DetailPage(int id, [FromServices] DbWebFinal db)
        {
            ViewData["Size"] = db.variant.Where(x => x.productsId == id).ToList();
            ViewData["Products"] = db.products.Where(c => c.id == id).ToList();
            ViewData["Images"] = db.productImages.Where(x => x.productsid == id).ToList();
            ViewData["Category"] = db.categories.ToList();
            ViewData["MayLike"] = db.products.Take(5).ToList();
            return View();
        }
        public IActionResult ShowOrders([FromServices] DbWebFinal db)
        {
            var userId = userService.GetUserId();
            ViewData["Orders"] = db.ordersDetails.Include(x => x.orders).Where(x => x.UserId == userId).ToList();
            ViewData["Category"] = db.categories.ToList();
            return View();
        }
        public IActionResult CategoryPage([FromServices] DbWebFinal db, int id, int page = 1, string sort = "all")
        {
            int pageSize = 9; // change as needed
            var query = db.variant
                .Include(x => x.products)
                .Where(x => x.categoryId == id);

            // Apply sorting based on the 'sort' parameter
            switch (sort.ToLower())
            {
                case "cheapest":
                    query = query.OrderBy(x => x.products.Price); // Sort by cheapest
                    break;
                case "expensive":
                    query = query.OrderByDescending(x => x.products.Price); // Sort by most expensive
                    break;
                case "all":
                default:
                    // No sorting or any default sorting (e.g., by latest) 
                    // Add sorting logic here if you want default sorting
                    break;
            }

            // Pagination
            int totalItems = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Pass the necessary data to the view
            ViewData["Categoryy"] = items;
            ViewData["Category"] = db.categories.ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CategoryId = id;
            ViewBag.SortOrder = sort; // Pass the selected sort order for reference in the view

            return View();
        }
    }
}
