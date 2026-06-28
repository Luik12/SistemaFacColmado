using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColmadoFacturacion.Models.Entities;

public class CierreDiario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CierreID { get; set; }
    public DateOnly Fecha { get; set; }
    public decimal TotalVentas { get; set; }
    public decimal TotalFacturado { get; set; }
    public int CantidadVentas { get; set; }
    public string? Observacion { get; set; }
    public int RealizadoPor { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public Usuario? Usuario { get; set; }
}