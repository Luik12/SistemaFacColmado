namespace ColmadoFacturacion.Models.Entities;

public class Empleado
{
    public int EmpleadoID { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Activo { get; set; } = true;
    public DateOnly FechaIngreso { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}