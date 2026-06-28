using ColmadoFacturacion.Models.Entities;
using ColmadoFacturacion.Models.ViewModels;

namespace ColmadoFacturacion.Services;

public interface IFacturaService
{
    Task<Venta> RegistrarVentaAsync(VentaVM ventaVM);
    Task<Factura> EmitirFacturaAsync(int ventaID);
    Task AnularVentaAsync(int ventaID, int usuarioID);
}