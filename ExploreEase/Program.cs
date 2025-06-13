using DataAcessLayer.DataAcess;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Interfaces;
using Repository.Repository;
using Services.Interfaces;
using Services.Services;
var builder = WebApplication.CreateBuilder(args);
// Connection string retrieval
var connectionString = builder.Configuration.GetConnectionString("ExploreEaseDbContextConnection")
                        ?? throw new InvalidOperationException("Connection string 'ExploreEaseDbContextConnection' not found.");
// Adding DbContext for SQL Server
builder.Services.AddDbContext<ExploreEaseDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.MigrationsAssembly("ExploreEase")));
// Adding Identity services
builder.Services.AddDefaultIdentity<ExploreEaseUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ExploreEaseDbContext>();
// Registering services for DI
builder.Services.AddTransient<ImageSaveService>();
builder.Services.AddTransient<TourRepository>();
builder.Services.AddTransient<TourServices>();
builder.Services.AddTransient<GetServicesRepository>();
builder.Services.AddTransient<GetServices>();
builder.Services.AddTransient<BookingDetailRepo>();
builder.Services.AddTransient<BookingDetails>();
builder.Services.AddScoped<KhaltiService>();
builder.Services.AddScoped<PaymentRepo>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<RecommendationRepo>();
builder.Services.AddScoped<RecommendationService>();
builder.Services.AddScoped<OrderDetailRepo>();
builder.Services.AddScoped<OrderDetailServices>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<ReviewServices>();
builder.Logging.AddConsole();
// Adding MVC support (controllers with views)
builder.Services.AddControllersWithViews();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
}); 
builder.Services.AddHttpClient();
// ... other service registrations ...
// Build the app after services registration
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Enable detailed error pages in development mode
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
// Map routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "{area=Admin}/{controller=Admin}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "UserActivity",
    areaName: "UserActivity",
    pattern: "{area=UserActivity}/{controller=User}/{action=Index}/{id?}");
// Role and User initialization
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}
app.MapRazorPages();
app.Run();
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ExploreEaseUser>>();
    string[] roleNames = { "Admin", "Manager", "User" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
    string fullName = "Admin";
    string adminEmail = "Admin413@gmail.com";
    string adminPassword = "Admin@123";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdminUser = new ExploreEaseUser
        {
            FullName = fullName,
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var createUserResult = await userManager.CreateAsync(newAdminUser, adminPassword);

        if (createUserResult.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdminUser, "Admin");
        }
    }
    else
    {
        var isInRole = await userManager.IsInRoleAsync(adminUser, "Admin");
        if (!isInRole)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
