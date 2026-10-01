namespace GestionLibros.DAL;

using GestionLibros.Models;
using Microsoft.EntityFrameworkCore;

public class Contexto : DbContext
{
    public DbSet<Libros> Libros { get; set; }
    public DbSet<Estudiantes> Estudiantes { get; set; }
    public DbSet<Prestamos> Prestamos { get; set; }

    public Contexto(DbContextOptions<Contexto> options) : base(options) { }
}