using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLibros.Models;

public class Prestamos
{
    [Key]
    public int PrestamoId { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    public int LibroId { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    public int EstudianteId { get; set; }

    public DateTime FechaPrestamo { get; set; } = DateTime.Now;

    public DateTime? FechaDevolucion { get; set; } = DateTime.Now;

    [ForeignKey(nameof(LibroId))]
    public virtual Libros Libro { get; set; } = null!;

    [ForeignKey(nameof(EstudianteId))]
    public virtual Estudiantes Estudiante { get; set; } = null!;
}