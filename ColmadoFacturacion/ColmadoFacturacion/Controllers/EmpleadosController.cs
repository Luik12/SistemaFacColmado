using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Controllers;

[Authorize]
public class EmpleadosController : Controller
{
    private readonly AppDbContext _context;
    public EmpleadosController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var empleados = await _context.Empleados
            .Where(e => e.Activo)
            .OrderBy(e => e.Nombre)
            .ToListAsync();
        return View(empleados);
    }

    [HttpGet]
    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(Empleado empleado)
    {
        if (!ModelState.IsValid) return View(empleado);

        _context.Empleados.Add(empleado);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Empleado creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado == null) return NotFound();
        return View(empleado);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Empleado empleado)
    {
        if (!ModelState.IsValid) return View(empleado);

        _context.Empleados.Update(empleado);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Empleado actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado == null) return NotFound();
        empleado.Activo = false;
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Empleado desactivado.";
        return RedirectToAction(nameof(Index));
    }
}