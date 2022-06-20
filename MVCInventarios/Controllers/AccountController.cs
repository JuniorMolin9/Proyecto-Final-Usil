using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCInventarios.Data;
using MVCInventarios.Models;
using MVCInventarios.ViewModels;
using System.Security.Claims;

namespace MVCInventarios.Controllers;
public class AccountController : Controller
{

    private readonly InventariosContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly INotyfService _servicioNotificacion;

    public AccountController(InventariosContext context, IPasswordHasher<Usuario> passwordHasher,
    INotyfService servicioNotificacion)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _servicioNotificacion = servicioNotificacion;
    }


    public IActionResult Login(string returnUrl)
    {
        LoginViewModel viewModel = new LoginViewModel();
        viewModel.ReturnUrl = returnUrl;
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        viewModel.ReturnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {

            var usuarioBd = _context.Usuarios
                .Include(u => u.Perfil)
                .FirstOrDefault(u => u.Username.ToLower().Trim() == viewModel.Username.ToLower().Trim());

            if (usuarioBd == null)
            {
                ModelState.AddModelError("", "Lo sentimos, el usuario no existe");
                _servicioNotificacion.Warning("Lo sentimos, el usuario no existe.");
                return View(viewModel);
            }

            var result = _passwordHasher.VerifyHashedPassword(usuarioBd, usuarioBd.Contrasena, viewModel.Password);

            if (result == PasswordVerificationResult.Success)
            {
                //La contraseña es correcta

                //Claim es un fragmento de información del usuario, en este caso
                //agregamos su username, su nombre completo y el nombre de su perfil.
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioBd.Username),
                        new Claim("FullName", usuarioBd.Nombre + " " + usuarioBd.Apellidos),
                        new Claim(ClaimTypes.Role,usuarioBd.Perfil.Nombre)
                    };

                //El ClaimIdentity es el contenedor de todos los claims del usuario.
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //AuthenticacionPropertyes es un diccionario de datos utilizado
                //para almacenar valores relacionados con la sesión de autenticación.
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,                //Permite que se refresque el tiempo de la sesión de autenticación
                    IsPersistent = viewModel.Recordarme //Establece si la sesión de autenticación
                                                        //puede ser persistente a través de múltiples peticiones
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return LocalRedirect(viewModel.ReturnUrl);
            }
            else
            {
                _servicioNotificacion.Warning("La contraseña es incorrecta");
                return View(viewModel);
            }


        }
        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }


    public ViewResult AccesoDenegado()
    {
        return View();
    }


}
