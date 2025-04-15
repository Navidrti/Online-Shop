using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Online_Shop.Areas.Identity.Data;
using Online_Shop.Data;
using Online_Shop.Models;
using Online_Shop.Service;
using Online_Shop.ViewModels;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Online_Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IActionResult> RegisterConfirm(RegisterViewModels models, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(models.username);
            if (user == null)
            {
                user = new ApplicationUser();
                user.UserName = models.username;
                user.Email = models.username;
                user.EmailConfirmed = true;
            }
            await userManager.CreateAsync(user, models.password);
            if (await userManager.IsInRoleAsync(user, "Customer") == false)
            {
                await userManager.AddToRoleAsync(user, "Customer");
            }

            return RedirectToAction("SignIn", "Home");
        }
        public async Task<IActionResult> Login(LoginViewModels models, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] SignInManager<ApplicationUser> signInManager)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(models.username);
            if (user == null)
            {
                return RedirectToAction("SignUp", "Home");
            }
            else
            {
                var success = await signInManager.PasswordSignInAsync(user, models.password, false, false);
                if (success.Succeeded)
                {
                    if (await userManager.IsInRoleAsync(user, "Admin") == true)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("SignUp", "Home");
                }
            }
        }
        public async Task<IActionResult> Logout([FromServices] SignInManager<ApplicationUser> signInManager)
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Insert([FromServices] DbWebFinal db, ProductsViewModels p)
        {
            Products products = new Products();
            products.Name = p.Name;
            products.Price = p.Price;
            products.description = p.description;
            if (p.img != null)
            {
                byte[] b = new byte[p.img.Length];
                p.img.OpenReadStream().Read(b, 0, b.Length);
                products.img = b;
            }
            List<ProductImages> lstImages = new List<ProductImages>();
            p.image.ForEach(x =>
            {
                if (x != null)
                {
                    byte[] b = new byte[x.Length];
                    x.OpenReadStream().Read(b, 0, b.Length);
                    MemoryStream memory = new MemoryStream(b);
                    Image imagefile = Image.FromStream(memory);
                    Bitmap bitmap = new Bitmap(imagefile, 200, 200 * imagefile.Height / imagefile.Width);
                    MemoryStream memory1 = new MemoryStream();
                    bitmap.Save(memory1, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ProductImages images = new ProductImages();
                    images.img = b;
                    images.thumbnailimg = memory1.ToArray();
                    lstImages.Add(images);
                }
            });
            products.productImages = lstImages;
            db.Add(products);
            db.SaveChanges();
            return RedirectToAction("Insert", "Admin");
        }
        public IActionResult InsertVariant([FromServices] DbWebFinal db, VariantViewModels v)
        {
            Variant variant = new Variant();
            variant.size = v.size;
            variant.color = v.color;
            variant.count = v.count;
            variant.productsId = v.productsId;
            variant.categoryId = v.categoryId;
            variant.category = db.categories.FirstOrDefault(x=>x.id == v.categoryId).Name;
            db.Add(variant);
            db.SaveChanges();
            return RedirectToAction("AddVariant", "Admin");
        }
        public IActionResult AddToCart(int id, [FromServices] DbWebFinal db, CartViewModels c, Variant v)
        {
            Cart cart = new Cart();
            var userId = userService.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                var pp = db.products.FirstOrDefault(x => x.id == id).Name;
                cart.Name = pp;
                cart.sizeId = c.sizeId;
                cart.Size = db.variant.FirstOrDefault(x => x.Id == cart.sizeId).size;
                cart.colorId = c.colorId;
                cart.Color = db.variant.FirstOrDefault(x => x.Id == cart.sizeId).color;
                cart.img = db.products.FirstOrDefault(x => x.id == id).img;
                cart.Count = c.count;
                cart.price = db.products.FirstOrDefault(x => x.id == id).Price;
                cart.totalPrice = cart.price * cart.Count;
                cart.Userid = userId;
                cart.UserName = db.Users.FirstOrDefault(x => x.Id == cart.Userid).UserName;
                cart.productsId = id;

            }
            var g = db.variant.FirstOrDefault(x => x.Id == cart.sizeId);
            //g.count -= cart.Count;
            db.Add(cart);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ShowCart([FromServices] DbWebFinal db)
        {
            ViewData["Category"] = db.categories.ToList();
            var userId = userService.GetUserId();



            ViewData["Cart"] = db.cart.Where(x => x.Userid == userId).ToList();
            Cart c = new Cart();
            var p = db.cart.Where(x => x.Userid == userId).ToList();
            var g = p.Sum(x => x.totalPrice);
            c.totalPrice = g;
            return View(c);


        }
        public IActionResult DeleteCart([FromServices] DbWebFinal db, int id)
        {
            var p = db.cart.Find(id);
            db.Remove(p);
            db.SaveChanges();
            return RedirectToAction("ShowCart", "Account");
        }
        public IActionResult Delete(int id, [FromServices] DbWebFinal db)
        {
            var p = db.products.Find(id);
            db.Remove(p);
            db.SaveChanges();
            return RedirectToAction("ShowProducts", "Admin");
        }
        public IActionResult Search(string searchString, [FromServices] DbWebFinal db,int page = 1, int pageSize = 9, string sortBy = "all")
        {
            ViewData["Category"] = db.categories.ToList();

            var products = db.products.AsQueryable();

            // Apply search filter if search string is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                products = products.Where(p => p.Name.ToLower().Contains(searchString));
            }

            // Apply sorting based on sortBy parameter
            switch (sortBy.ToLower())
            {
                case "cheapest":
                    products = products.OrderBy(p => p.Price); // Sort by cheapest price
                    break;
                case "mostexpensive":
                    products = products.OrderByDescending(p => p.Price); // Sort by most expensive price
                    break;
                case "all":
                default:
                    // No sorting or default sorting
                    products = products.OrderBy(p => p.Name); // Default sorting by name (or any default logic you prefer)
                    break;
            }

            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Pass data to the view
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["SearchString"] = searchString;
            ViewData["SortBy"] = sortBy;  // Pass the selected sort option

            return View(paginatedProducts);
        }
        public IActionResult PlaceOrder([FromServices] DbWebFinal db, OrdersViewModels o)
        {
            Orders oo = new Orders();
            oo.Name = o.Name;
            oo.lastName = o.lastName;
            oo.Email = o.Email;
            oo.Mobile = o.Mobile;
            oo.Address = o.Address;
            oo.Country = o.Country;
            oo.City = o.City;
            var userId = userService.GetUserId();
            var p = db.cart.Where(x => x.Userid == userId).ToList();
            List<OrdersDetail> lstOrders = new List<OrdersDetail>();
            p.ForEach(x =>
            {
                OrdersDetail o = new OrdersDetail();
                o.productName = x.Name;
                o.Size = x.Size;
                o.Color = x.Color;
                o.Count = x.Count;
                o.Price = x.price;
                o.totalPrice = x.totalPrice;
                o.UserName = x.UserName;
                o.UserId = userId;
                o.img = x.img;
                lstOrders.Add(o);
            });
            oo.ordersDetails = lstOrders;
            db.Add(oo);
            var q = db.cart.Where(x=>x.Userid == userId).ToList();
            db.cart.RemoveRange(q);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult InsertCategory([FromServices] DbWebFinal db,CategoryViewModels c)
        {
            Category cat = new Category();
            cat.Name = c.Name;
            if (c.img != null)
            {
                byte[] b = new byte[c.img.Length];
                c.img.OpenReadStream().Read(b, 0, b.Length);
                cat.img = b;
            }
            db.Add(cat);
            db.SaveChanges();
            return RedirectToAction("AddCategory", "Admin");
        }
        public IActionResult ContactUs([FromServices] DbWebFinal db,ContactUsViewModels cu)
        {
            ContactUs c = new ContactUs();
            c.Name = cu.Name;
            c.Email = cu.Email;
            c.Subject = cu.Subject;
            c.Message = cu.Message;
            db.Add(c);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
