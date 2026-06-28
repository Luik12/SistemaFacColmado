namespace ColmadoFacturacion.Services;

public interface IStockService
{
    Task<bool> HayCierrePendienteAsync(DateOnly fecha);
    Task<int> RealizarCierreDiarioAsync(DateOnly fecha, int usuarioID, string? obs = null);
    Task AgregarStockAsync(int productoID, int cantidad, int usuarioID, string referencia);
}