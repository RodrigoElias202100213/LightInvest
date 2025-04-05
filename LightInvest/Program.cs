using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LightInvest.Models.Email;
using LightInvest.Models.BD;
using LightInvest.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<MediaStackService>();
builder.Services.AddScoped<MediaStackService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/SplashScreen/SplashScreen";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

builder.Services.AddSingleton<EmailService>();

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

var env = app.Services.GetRequiredService<IWebHostEnvironment>();
string rotativaPath = Path.Combine(env.WebRootPath, "rotativa", "wkhtmltopdf.exe");

if (!File.Exists(rotativaPath))
{
    throw new FileNotFoundException($"Erro: 'wkhtmltopdf.exe' não foi encontrado em {rotativaPath}. Verifique o caminho e mova o arquivo para o local correto.");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SplashScreen}/{action=SplashScreen}"
);

app.Run();

public partial class Program { }
