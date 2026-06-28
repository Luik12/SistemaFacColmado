using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuariosController : Controller
{
    private readonly AppDbContext _context;
    public UsuariosController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var usuarios = await _context.Usuarios
            .Include(u => u.Rol)
            .OrderBy(u => u.NombreUsuario)
            .ToListAsync();
        return View(usuarios);
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "RolID", "Nombre");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(string NombreUsuario, string Email, string Password, int RolID)
    {
        if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == NombreUsuario))
        {
            ModelState.AddModelError("", "Ese nombre de usuario ya existe.");
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "RolID", "Nombre");
            return View();
        }

        var usuario = new Usuario
        {
            NombreUsuario = NombreUsuario,
            Email = Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
            RolID = RolID,
            Activo = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        TempData["Exito"] = $"Usuario '{NombreUsuario}' creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();
        ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "RolID", "Nombre", usuario.RolID);
        return View(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int UsuarioID, string NombreUsuario, string Email, int RolID, bool Activo)
    {
        var usuario = await _context.Usuarios.FindAsync(UsuarioID);
        if (usuario == null) return NotFound();

        usuario.NombreUsuario = NombreUsuario;
        usuario.Email = Email;
        usuario.RolID = RolID;
        usuario.Activo = Activo;

        await _context.SaveChangesAsync();
        TempData["Exito"] = "Usuario actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> CambiarPassword(int id, string NuevaPassword)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NuevaPassword);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Contraseña actualizada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Desactivar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();
        usuario.Activo = false;
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Usuario desactivado.";
        return RedirectToAction(nameof(Index));
    }
}