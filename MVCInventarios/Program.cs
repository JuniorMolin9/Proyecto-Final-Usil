using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCInventarios.Data;
using MVCInventarios.Helpers;
using MVCInventarios.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventariosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventariosContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/AccesoDenegado";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
    });


builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("Administradores",
                    policy => policy.RequireRole("Administrador"));


    options.AddPolicy("EmpleadosEmpresa",
                    policy => policy.RequireRole("Administrador","Empleado"));


    options.AddPolicy("Organizacion",
                    policy => policy.RequireRole("Administrador","Empleado","Servicio Social"));


});


builder.Services.AddNotyf(config => {
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddSingleton<UsuarioFactoria>();
builder.Services.AddSingleton<ProductoFactoria>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<InventariosContext>();
        DbInitializer.Initialize(context);
    }



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();