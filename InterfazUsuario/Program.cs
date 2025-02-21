using InterfazUsuario.Interfaces;
using InterfazUsuario.Managers;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();

// Configuraci�n de servicios
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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configuraci�n de endpoints simplificada
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();