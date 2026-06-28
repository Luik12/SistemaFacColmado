using System.ComponentModel.DataAnnotations;

namespace ColmadoFacturacion.Models.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "El usuario es requerido")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}