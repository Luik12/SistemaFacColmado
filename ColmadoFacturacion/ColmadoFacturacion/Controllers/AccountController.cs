using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ColmadoFacturacion.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    public AccountController(AppDbContext context) => _context = context;

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.NombreUsuario == model.NombreUsuario && u.Activo);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.PasswordHash))
        {
            ModelState.AddModelError("", "Credenciales inválidas");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
            new(ClaimTypes.Name, usuario.NombreUsuario),
            new(ClaimTypes.Role, usuario.Rol!.Nombre),
            new("UsuarioID", usuario.UsuarioID.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}