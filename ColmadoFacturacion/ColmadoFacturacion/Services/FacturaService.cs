using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.Entities;
using ColmadoFacturacion.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Services;

public class FacturaService : IFacturaService
{
    private readonly AppDbContext _context;

    public FacturaService(AppDbContext context) => _context = context;

    public async Task<Venta> RegistrarVentaAsync(VentaVM vm)
    {
        var venta = new Venta
        {
            EmpleadoID = vm.EmpleadoID,
            ClienteID = vm.ClienteID,
            Facturada = vm.RequiereFactura,
            Observacion = vm.Observacion,
            Detalles = new List<DetalleVenta>()
        };

        decimal subtotal = 0;
        foreach (var d in vm.Detalles)
        {
            var linea = new DetalleVenta
            {
                ProductoID = d.ProductoID,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento,
                Subtotal = (d.PrecioUnitario * d.Cantidad) - d.Descuento
            };
            subtotal += linea.Subtotal;
            venta.Detalles.Add(linea);
        }

        venta.Subtotal = subtotal;
        venta.ITBIS = vm.RequiereFactura ? subtotal * 0.18m : 0; // 18% ITBIS solo en facturas
        venta.Descuento = 0;
        venta.Total = subtotal + venta.ITBIS;

        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();

        // Si requiere factura, generarla automáticamente
        if (vm.RequiereFactura && vm.ClienteID.HasValue)
            await EmitirFacturaAsync(venta.VentaID);

        return venta;
    }

    public async Task<Factura> EmitirFacturaAsync(int ventaID)
    {
        var venta = await _context.Ventas
            .Include(v => v.Cliente)
            .FirstOrDefaultAsync(v => v.VentaID == ventaID)
            ?? throw new Exception("Venta no encontrada");

        if (venta.ClienteID == null)
            throw new Exception("La venta no tiene cliente asignado para facturar");

        // Generar número de factura secuencial
        int ultimo = await _context.Facturas.CountAsync();
        string numero = $"F-{DateTime.Now.Year}-{(ultimo + 1):D5}";

        var factura = new Factura
        {
            NumeroFactura = numero,
            VentaID = ventaID,
            ClienteID = venta.ClienteID!.Value,
            Subtotal = venta.Subtotal,
            Descuento = venta.Descuento,
            ITBIS = venta.ITBIS,
            Total = venta.Total
        };

        venta.Facturada = true;
        _context.Facturas.Add(factura);
        await _context.SaveChangesAsync();

        return factura;
    }

    public async Task AnularVentaAsync(int ventaID, int usuarioID)
    {
        var venta = await _context.Ventas
            .Include(v => v.Factura)
            .FirstOrDefaultAsync(v => v.VentaID == ventaID)
            ?? throw new Exception("Venta no encontrada");

        venta.Anulada = true;
        if (venta.Factura != null) venta.Factura.Anulada = true;

        await _context.SaveChangesAsync();
    }
}