using System.ComponentModel.DataAnnotations;

namespace GestionLibros.Models;

public class Estudiantes
{
    [Key]
    public int EstudianteId { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    public string Direccion { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    [EmailAddress(ErrorMessage = "Email no válido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    public DateTime FechaNacimiento { get; set; }
}