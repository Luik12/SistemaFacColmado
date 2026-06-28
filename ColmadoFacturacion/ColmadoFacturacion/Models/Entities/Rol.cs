namespace ColmadoFacturacion.Models.Entities;

public class Rol
{
    public int RolID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}