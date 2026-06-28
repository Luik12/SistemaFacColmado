using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.ViewModels;
using ColmadoFacturacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ColmadoFacturacion.Controllers;

[Authorize]
public class VentasController : Controller
{
    private readonly AppDbContext _context;
    private readonly IFacturaService _facturaService;

    public VentasController(AppDbContext context, IFacturaService facturaService)
    {
        _context = context;
        _facturaService = facturaService;
    }

    public async Task<IActionResult> Index()
    {
        var ventas = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Empleado)
            .OrderByDescending(v => v.Fecha)
            .Take(100)
            .ToListAsync();
        return View(ventas);
    }

    [HttpGet]
    public async Task<IActionResult> Nueva()
    {
        ViewBag.Clientes = await _context.Clientes.Where(c => c.Activo).ToListAsync();
        ViewBag.Empleados = await _context.Empleados.Where(e => e.Activo).ToListAsync();
        ViewBag.Productos = await _context.Productos
            .Where(p => p.Activo && p.Stock > 0)
            .Select(p => new { p.ProductoID, p.Nombre, p.PrecioVenta, p.Stock })
            .ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Nueva([FromBody] VentaVM ventaVM)
    {
        try
        {
            var venta = await _facturaService.RegistrarVentaAsync(ventaVM);
            return Json(new { success = true, ventaID = venta.VentaID });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Supervisor")]
    public async Task<IActionResult> Anular(int id)
    {
        var usuarioID = int.Parse(User.FindFirstValue("UsuarioID")!);
        await _facturaService.AnularVentaAsync(id, usuarioID);
        TempData["Exito"] = "Venta anulada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var venta = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Empleado)
            .Include(v => v.Detalles).ThenInclude(d => d.Producto)
            .Include(v => v.Factura)
            .FirstOrDefaultAsync(v => v.VentaID == id);

        if (venta == null) return NotFound();
        return View(venta);
    }
}