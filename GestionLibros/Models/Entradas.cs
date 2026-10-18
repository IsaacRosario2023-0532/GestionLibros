using System.ComponentModel.DataAnnotations;

namespace GestionLibros.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Campo Requerido")]
    public string Concepto { get; set; } = string.Empty;

    public int Total { get; set; }

    public ICollection<EntradasDetalle> EntradasDetalles { get; set; } = new List<EntradasDetalle>();
}