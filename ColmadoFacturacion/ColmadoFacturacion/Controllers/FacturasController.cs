using ColmadoFacturacion.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Controllers;

[Authorize]
public class FacturasController : Controller
{
    private readonly AppDbContext _context;
    public FacturasController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var facturas = await _context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Venta)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
        return View(facturas);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var factura = await _context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Venta).ThenInclude(v => v!.Detalles).ThenInclude(d => d.Producto)
            .Include(f => f.Venta).ThenInclude(v => v!.Empleado)
            .FirstOrDefaultAsync(f => f.FacturaID == id);

        if (factura == null) return NotFound();
        return View(factura);
    }
}