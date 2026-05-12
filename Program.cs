using Microsoft.EntityFrameworkCore;
using Desafio_2.Data;
using ColegioSanJose.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();

// 1. Registramos ColegioDbContext (Para Alumnos, Materias, etc.)
builder.Services.AddDbContext<ColegioDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registramos ApplicationDbContext (Para el HomeController y Seguridad)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Sesión
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Pipeline de solicitudes
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Migraciones automáticas
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    // Aplicamos migración para el contexto de la aplicación
    var dbApp = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbApp.Database.Migrate();

    // Aplicamos migración para el contexto del colegio
    var dbColegio = scope.ServiceProvider.GetRequiredService<ColegioDbContext>();
    dbColegio.Database.Migrate();
}

app.Run();