



using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Areas.Identity.Data;
using Online_Shop.Controllers;
using Online_Shop.Data;
using Online_Shop.Service;



namespace Online_Shop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<DbWebFinal>();
            services.AddScoped<IUserService, UserService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager, [FromServices] DbWebFinal db)

        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();





            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            InitIdentity(userManager,roleManager,db).Wait();
        }
        private async Task InitIdentity([FromServices] UserManager<ApplicationUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager, [FromServices] DbWebFinal db)
        {
            db.Database.EnsureCreated();
            ApplicationUser admin = await userManager.FindByNameAsync("Navid");
            if (admin == null)
            {
                admin = new ApplicationUser();
                admin.UserName = "Navid";
                admin.Email = "Navid";
                admin.EmailConfirmed = true;
            }
            await userManager.CreateAsync(admin, "Realmadrid7_a");
            if (await roleManager.RoleExistsAsync("Admin") == false)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.RoleExistsAsync("Customer") == false)
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            if (await userManager.IsInRoleAsync(admin, "Admin") == false)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

    }
}
