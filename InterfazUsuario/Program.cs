using InterfazUsuario.Interfaces;
using InterfazUsuario.Managers;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();

var emailSettings = new
{
    SmtpServer = Environment.GetEnvironmentVariable("EMAIL_HOST"),
    Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT") ?? "587"),
    Username = Environment.GetEnvironmentVariable("EMAIL_USER"),
    Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")
};

// Configuración de servicios
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRespuestasManager, RespuestasManager>();
builder.Services.AddTransient<IEmailSender, EmailManager>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configuración de endpoints simplificada
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();