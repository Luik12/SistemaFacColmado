namespace ColmadoFacturacion.Models.ViewModels;

public class VentaVM
{
    public int? ClienteID { get; set; }
    public int EmpleadoID { get; set; }
    public bool RequiereFactura { get; set; } = false;
    public string? Observacion { get; set; }
    public List<DetalleVentaVM> Detalles { get; set; } = new();
}

public class DetalleVentaVM
{
    public int ProductoID { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; } = 0;
}