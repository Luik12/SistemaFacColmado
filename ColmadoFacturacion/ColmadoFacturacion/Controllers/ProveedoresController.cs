using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Controllers;

[Authorize]
public class ProveedoresController : Controller
{
    private readonly AppDbContext _context;
    public ProveedoresController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var proveedores = await _context.Proveedores
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
        return View(proveedores);
    }

    [HttpGet]
    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(Proveedor proveedor)
    {
        if (!ModelState.IsValid) return View(proveedor);

        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Proveedor creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor == null) return NotFound();
        return View(proveedor);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Proveedor proveedor)
    {
        if (!ModelState.IsValid) return View(proveedor);

        _context.Proveedores.Update(proveedor);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Proveedor actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor == null) return NotFound();
        proveedor.Activo = false;
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Proveedor desactivado.";
        return RedirectToAction(nameof(Index));
    }
}