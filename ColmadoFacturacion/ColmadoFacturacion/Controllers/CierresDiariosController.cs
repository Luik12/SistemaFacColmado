using ColmadoFacturacion.Data;
using ColmadoFacturacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ColmadoFacturacion.Controllers;

[Authorize(Roles = "Administrador,Supervisor")]
public class CierresDiariosController : Controller
{
    private readonly AppDbContext _context;
    private readonly IStockService _stockService; 

    public CierresDiariosController(AppDbContext context, IStockService stockService)
    {
        _context = context;
        _stockService = stockService;
    }

    public async Task<IActionResult> Index()
    {
        var cierres = await _context.CierresDiarios
            .Include(c => c.Usuario)
            .OrderByDescending(c => c.Fecha)
            .ToListAsync();
        return View(cierres);
    }

    [HttpGet]
    public async Task<IActionResult> Realizar()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        ViewBag.FechaHoy = hoy;
        ViewBag.YaRealizado = await _stockService.HayCierrePendienteAsync(hoy);

        // Pre-calcular resumen del día
        var ventas = await _context.Ventas
            .Where(v => v.Fecha.Date == DateTime.Today && !v.Anulada)
            .ToListAsync();

        ViewBag.TotalVentas = ventas.Sum(v => v.Total);
        ViewBag.CantidadVentas = ventas.Count;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Realizar(string? observacion)
    {
        var usuarioID = int.Parse(User.FindFirstValue("UsuarioID")!);
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        try
        {
            await _stockService.RealizarCierreDiarioAsync(hoy, usuarioID, observacion);
            TempData["Exito"] = $"Cierre del día {hoy:dd/MM/yyyy} realizado exitosamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}