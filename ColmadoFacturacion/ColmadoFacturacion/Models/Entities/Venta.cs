namespace ColmadoFacturacion.Models.Entities;

public class Venta
{
    public int VentaID { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int EmpleadoID { get; set; }
    public int? ClienteID { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; } = 0;
    public decimal ITBIS { get; set; } = 0;
    public decimal Total { get; set; }
    public string? Observacion { get; set; }
    public bool Facturada { get; set; } = false;
    public bool Anulada { get; set; } = false;

    public Empleado? Empleado { get; set; }
    public Cliente? Cliente { get; set; }
    public Factura? Factura { get; set; }
    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}