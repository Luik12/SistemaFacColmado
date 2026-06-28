namespace ColmadoFacturacion.Models.Entities;

public class Factura
{
    public int FacturaID { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public int VentaID { get; set; }
    public int ClienteID { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.Now;
    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; } = 0;
    public decimal ITBIS { get; set; } = 0;
    public decimal Total { get; set; }
    public bool Anulada { get; set; } = false;
    public string? Observacion { get; set; }

    public Venta? Venta { get; set; }
    public Cliente? Cliente { get; set; }
}