using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Areas.Identity.Data;
using Online_Shop.Data;
using Online_Shop.ViewModels;

namespace Online_Shop.Controllers
{
    [Authorize(Policy =("AdminPolicy"))]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Insert([FromServices] DbWebFinal db)
        {
          
            
            return View();
        }
        public IActionResult ShowProducts([FromServices] DbWebFinal db)
        {
            var p = db.variant.Include(x => x.products).ToList();
            return View(p);
        }
        public IActionResult ListOfUsers([FromServices] DbWebFinal db, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager)
        {
            var users = db.Users.ToList();
            var userroles = db.UserRoles.ToList();
            var roles = db.Roles.ToList();
            List<UsersViewModels> lstusers = new List<UsersViewModels>();
            users.ForEach(x =>
            {
                var userrole = userroles.Where(y => y.UserId == x.Id).ToList();
                userrole.ForEach(k =>
                {
                    var rolename = roles.Where(h => h.Id == k.RoleId).ToList();
                    rolename.ForEach(m =>
                    {
                        UsersViewModels usersView = new UsersViewModels();
                        usersView.username = x.UserName;
                        usersView.roleName = m.Name;
                        lstusers.Add(usersView);
                    });
                });
            });
            return View(lstusers);
        }
        public IActionResult Orders([FromServices] DbWebFinal db)
        {
            ViewData["Orders"] =  db.ordersDetails.Include(x=>x.orders).ToList();
            return View();
        }
        public IActionResult AddVariant([FromServices] DbWebFinal db)
        {
            ViewData["Products"] = db.products.ToList();
            ViewData["Category"] = db.categories.ToList();
            return View();
        }
        public IActionResult AddColor()
        {
            return View();
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        public IActionResult ShowContactUs([FromServices] DbWebFinal db)
        {
            ViewData["Contacts"] = db.contactUs.ToList();
            return View();
        }
    }
}
