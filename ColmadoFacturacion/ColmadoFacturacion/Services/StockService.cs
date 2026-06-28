using ColmadoFacturacion.Data;
using ColmadoFacturacion.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Services;

public class StockService : IStockService
{
    private readonly AppDbContext _context;

    public StockService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HayCierrePendienteAsync(DateOnly fecha)
    {
        return await _context.CierresDiarios
            .AnyAsync(c => c.Fecha == fecha);
    }

    public async Task<int> RealizarCierreDiarioAsync(DateOnly fecha, int usuarioID, string? obs = null)
    {
        return await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_RealizarCierreDiario @Fecha, @UsuarioID, @Observacion",
            new SqlParameter("@Fecha", fecha),
            new SqlParameter("@UsuarioID", usuarioID),
            new SqlParameter("@Observacion", (object?)obs ?? DBNull.Value)
        );
    }

    public async Task AgregarStockAsync(int productoID, int cantidad, int usuarioID, string referencia)
    {
        var producto = await _context.Productos.FindAsync(productoID);

        if (producto == null)
            throw new Exception("Producto no encontrado");

        var anterior = producto.Stock;
        producto.Stock += cantidad;

        _context.MovimientosStock.Add(new MovimientoStock
        {
            ProductoID = productoID,
            Tipo = "ENTRADA",
            Cantidad = cantidad,
            StockAnterior = anterior,
            StockNuevo = producto.Stock,
            Referencia = referencia,
            UsuarioID = usuarioID
        });

        await _context.SaveChangesAsync();
    }
}