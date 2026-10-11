using GestionLibros.DAL;
using GestionLibros.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionLibros.Services;

public class DevolucionesService(IDbContextFactory<Contexto> DbFactory) : IDevolucionesService
{
    public async Task<List<Prestamos>> ListarPendientes()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .Include(p => p.Libro)
            .Include(p => p.Estudiante)
            .Where(p => p.FechaDevolucion == null)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Devolver(int prestamoId, DateTime fechaDevolucion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

        if (prestamo == null
            || prestamo.FechaDevolucion != null
            || fechaDevolucion.Date < prestamo.FechaPrestamo.Date)
            return false;

        prestamo.FechaDevolucion = fechaDevolucion;
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<LibroPromedioDto>> ListarLibrosConPromedio()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var libros = await contexto.Libros.AsNoTracking().ToListAsync();
        var devueltos = await contexto.Prestamos.AsNoTracking()
            .Where(p => p.FechaDevolucion != null)
            .ToListAsync();

        return libros.Select(l =>
        {
            var dias = devueltos
                .Where(p => p.LibroId == l.LibroId)
                .Select(p => (p.FechaDevolucion!.Value.Date - p.FechaPrestamo.Date).TotalDays)
                .ToList();

            return new LibroPromedioDto
            {
                LibroId = l.LibroId,
                Titulo = l.Titulo,
                Autor = l.Autor,
                CantidadDevoluciones = dias.Count,
                PromedioDiasPrestado = dias.Count > 0 ? dias.Average() : 0
            };
        }).ToList();
    }
}