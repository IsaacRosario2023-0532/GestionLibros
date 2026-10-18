using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLibros.Models;

public class EntradasDetalle
{
    [Key]
    public int Id { get; set; }
    public int EntradaId { get; set; }
    public int LibroId { get; set; }
    public int Cantidad { get; set; }

    [ForeignKey(nameof(EntradaId))]
    public virtual Entradas? Entrada { get; set; }

    [ForeignKey(nameof(LibroId))]
    public virtual Libros? Libro { get; set; }

    public EntradasDetalle() { }

    public EntradasDetalle(int libroId, int cantidad)
    {
        LibroId = libroId;
        Cantidad = cantidad;
    }
}