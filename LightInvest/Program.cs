using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LightInvest.Models.Email;
using LightInvest.Models.BD;

var builder = WebApplication.CreateBuilder(args);

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

string rotativaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "rotativa", "wkhtmltopdf.exe");

if (!File.Exists(rotativaPath))
{
	throw new FileNotFoundException($"Erro: 'wkhtmltopdf.exe' não foi encontrado em {rotativaPath}. Verifique a pasta e mova o arquivo para o local correto.");
}
/*
// RotativaConfiguration.Setup(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "rotativa"));

// Verifique se o arquivo realmente existe
if (!File.Exists(rotativaPath))
{
	throw new FileNotFoundException($"O arquivo 'wkhtmltopdf.exe' não foi encontrado no caminho especificado: {rotativaPath}");
}
*/

// Adicionando os serviços necessários
//builder.Services.AddSingleton<PdfGenerator>(new PdfGenerator(rotativaPath));
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