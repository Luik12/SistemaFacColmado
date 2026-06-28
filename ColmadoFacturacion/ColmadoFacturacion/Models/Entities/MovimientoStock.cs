using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColmadoFacturacion.Models.Entities;

public class MovimientoStock
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MovimientoID { get; set; }

    public int ProductoID { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public int StockAnterior { get; set; }
    public int StockNuevo { get; set; }
    public string? Referencia { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int UsuarioID { get; set; }

    public Producto? Producto { get; set; }
    public Usuario? Usuario { get; set; }
}