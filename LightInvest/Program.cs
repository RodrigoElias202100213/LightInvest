using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LightInvest.Models.Email;
using LightInvest.Models.BD;
using LightInvest.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Adicionar MediaStackService ao contêiner de DI
builder.Services.AddHttpClient<MediaStackService>();
builder.Services.AddScoped<MediaStackService>();

// Configuração do DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

// Configuração do cookie de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/SplashScreen/SplashScreen";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

// Injetar IWebHostEnvironment para acessar o WebRootPath
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

// Adicionando os serviços necessários
builder.Services.AddSingleton<EmailService>();

// Configuração dos serviços para controladores e visualizações
builder.Services.AddControllersWithViews();

// Configuração de cache e sessão
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Verifique o caminho correto usando WebRootPath
var env = app.Services.GetRequiredService<IWebHostEnvironment>();
string rotativaPath = Path.Combine(env.WebRootPath, "rotativa", "wkhtmltopdf.exe");

if (!File.Exists(rotativaPath))
{
    throw new FileNotFoundException($"Erro: 'wkhtmltopdf.exe' não foi encontrado em {rotativaPath}. Verifique o caminho e mova o arquivo para o local correto.");
}

// Configuração de ambiente e pipeline de requisições
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

// Configuração da rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SplashScreen}/{action=SplashScreen}"
);

app.Run();

public partial class Program { }