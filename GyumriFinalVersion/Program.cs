using GyumriFinalVersion.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gyumri.Data.Models;
using Gyumri.Application.Interfaces;
using Gyumri.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// DB Context
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GyumriDB")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Add MVC (controllers & views)
builder.Services.AddControllersWithViews();

// Custom services
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<ISubcategory, SubcategoryService>();
builder.Services.AddScoped<IPlace, PlaceService>();

var app = builder.Build();

// SEED admin user/roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = "admin@admin.com";
    var adminPassword = "Admin123!";

    // Check if user exists
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminPassword);
        if (result.Succeeded)
        {
            Console.WriteLine("✅ Admin user created successfully.");
            await userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            Console.WriteLine("❌ Failed to create admin user:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($" - {error.Description}");
            }
        }
    }
    else
    {
        Console.WriteLine("ℹ️ Admin user already exists.");
    }
}



// Middleware setup
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // ✅ IMPORTANT
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
