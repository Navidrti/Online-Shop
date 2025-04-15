using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Areas.Identity.Data;
using Online_Shop.Models;

namespace Online_Shop.Data;

public class DbWebFinal : IdentityDbContext<ApplicationUser>
{
    public DbSet<Products> products { get; set; }
    public DbSet<Variant> variant { get; set; }
 
    public DbSet<Cart> cart { get; set; }
    public DbSet<ProductImages> productImages { get; set; }
    public DbSet<Orders> orders { get; set; }
    public DbSet<OrdersDetail> ordersDetails { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<ContactUs> contactUs { get; set; }
   



    public DbWebFinal(DbContextOptions<DbWebFinal> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
