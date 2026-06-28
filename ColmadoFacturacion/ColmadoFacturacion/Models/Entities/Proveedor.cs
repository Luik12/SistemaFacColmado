namespace ColmadoFacturacion.Models.Entities;

public class Proveedor
{
    public int ProveedorID { get; set; }
    public string? RNC { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Contacto { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}