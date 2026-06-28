using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Controllers;

[Authorize]
public class ClientesController : Controller
{
    private readonly AppDbContext _context;
    public ClientesController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var clientes = await _context.Clientes
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
        return View(clientes);
    }

    [HttpGet]
    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(Cliente cliente)
    {
        if (!ModelState.IsValid) return View(cliente);

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Cliente creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        return View(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Cliente cliente)
    {
        if (!ModelState.IsValid) return View(cliente);

        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Cliente actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        cliente.Activo = false;
        await _context.SaveChangesAsync();
        TempData["Exito"] = "Cliente desactivado.";
        return RedirectToAction(nameof(Index));
    }
}