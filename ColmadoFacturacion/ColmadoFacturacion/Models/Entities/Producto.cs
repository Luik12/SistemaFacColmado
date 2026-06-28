namespace ColmadoFacturacion.Models.Entities;

public class Producto
{
    public int ProductoID { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CategoriaID { get; set; }
    public int? ProveedorID { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public int Stock { get; set; } = 0;
    public int StockMinimo { get; set; } = 5;
    public bool Activo { get; set; } = true;

    public Categoria? Categoria { get; set; }
    public Proveedor? Proveedor { get; set; }
    public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
}