using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebGuarida.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar servicios de localización
builder.Services.AddLocalization();

// Configurar a Español - Bolivia
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("es-BO") };
    options.DefaultRequestCulture = new RequestCulture("es-BO");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GuaridaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = new PathString("/Account/Login"));

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 2. Activar localización ANTES de UseRouting()
app.UseRequestLocalization();

app.UseRouting();

app.UseCookiePolicy();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Vista para cliente
app.MapControllerRoute(
    name: "catalogo",
    pattern: "Catalogo",
    defaults: new { controller = "Home", action = "Catalogo" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
