namespace ColmadoFacturacion.Models.Entities;

public class Cliente
{
    public int ClienteID { get; set; }
    public string? Cedula { get; set; }
    public string? RNC { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public bool EsEmpresa { get; set; } = false;
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}