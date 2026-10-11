namespace GestionLibros.Models;

public class LibroPromedioDto
{
    public int LibroId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public int CantidadDevoluciones { get; set; }
    public double PromedioDiasPrestado { get; set; }
}