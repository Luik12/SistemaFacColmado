using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Controllers;

[Authorize]
public class ProductosController : Controller
{
    private readonly AppDbContext _context;
    public ProductosController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Proveedor)
            .Where(p => p.Activo)
            .ToListAsync();
        return View(productos);
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        await CargarSelectLists();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Producto producto)
    {
        if (!ModelState.IsValid) { await CargarSelectLists(); return View(producto); }

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Producto creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var p = await _context.Productos.FindAsync(id);
        if (p == null) return NotFound();
        await CargarSelectLists();
        return View(p);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Producto producto)
    {
        if (!ModelState.IsValid) { await CargarSelectLists(); return View(producto); }
        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Producto actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var p = await _context.Productos.FindAsync(id);
        if (p == null) return NotFound();
        p.Activo = false; // baja lógica
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Producto desactivado.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarSelectLists()
    {
        ViewBag.Categorias = new SelectList(await _context.Categorias.ToListAsync(), "CategoriaID", "Nombre");
        ViewBag.Proveedores = new SelectList(await _context.Proveedores.Where(p => p.Activo).ToListAsync(), "ProveedorID", "Nombre");
    }
}