namespace ColmadoFacturacion.Models.Entities;

public class Usuario
{
    public int UsuarioID { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RolID { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Navegación
    public Rol? Rol { get; set; }
}