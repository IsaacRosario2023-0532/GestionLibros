using System.ComponentModel.DataAnnotations;

namespace GestionLibros.Models;

public class Libros
{
    [Key]
    public int LibroId { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    public string Titulo { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    public string Autor { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "Año no valido")]
    public int AnoPublicacion { get; set; }
}
