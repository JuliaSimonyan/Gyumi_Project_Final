using GyumriFinalVersion.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gyumri.Data.Models;
using Gyumri.Application.Interfaces;
using Gyumri.Application.Services;
using Gyumri.Middleware;
using Gyumri.App.Interfaces;
using Gyumri.App.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// DB Context
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GyumriDB")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Account/Login";
    options.AccessDeniedPath = "/Admin/Account/AccessDenied";
});

// Add MVC (controllers & views)
builder.Services.AddControllersWithViews();

// Custom services
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<ISubcategory, SubcategoryService>();
builder.Services.AddScoped<IPlace, PlaceService>();
builder.Services.AddScoped<IArticle, ArticleService>();
builder.Services.AddLocalization();

builder.Services.AddDistributedMemoryCache(); // Required for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// SEED admin user/roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var context = services.GetRequiredService<ApplicationContext>();

    // ✅ Seed roles
    string[] roles = { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // ✅ Seed admin user
    var adminEmail = "admin@admin.com";
    var adminPassword = "Admin123!";
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
    var categoryService = services.GetRequiredService<ICategory>();
    await categoryService.SeedDefaultCategoriesAsync();

}

// Middleware setup
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

var supportedCultures = new[] { "en", "hy-AM", "ru-RU" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);
app.UseMiddleware<LocalizationMiddleware>();

app.UseAuthentication(); // ✅ IMPORTANT
app.UseAuthorization();

app.UseCookiePolicy();
// Routing
app.MapControllerRoute(
    name: "areas",
    pattern: "gyumri/secure-portal/where-the-content-is-created/{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
