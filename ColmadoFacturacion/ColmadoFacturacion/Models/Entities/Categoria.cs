namespace ColmadoFacturacion.Models.Entities;

public class Categoria
{
    public int CategoriaID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}

