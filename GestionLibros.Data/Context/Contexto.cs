using GestionLibros.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionLibros.Data.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }

    public DbSet<Libros> Libros { get; set; }
    public DbSet<Estudiantes> Estudiantes { get; set; }
    public DbSet<Prestamos> Prestamos { get; set; }
    public DbSet<LibroPromedioDto> Devoluciones { get; set; }
    public DbSet<Entradas> Entradas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Indicamos que este DTO no tiene clave primaria (tabla física)
        modelBuilder.Entity<LibroPromedioDto>().HasNoKey();
    }
}