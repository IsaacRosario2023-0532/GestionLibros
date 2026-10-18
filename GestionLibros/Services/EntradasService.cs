using System.Linq.Expressions;
using GestionLibros.DAL;
using GestionLibros.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionLibros.Services;

public class EntradasService(IDbContextFactory<Contexto> DbFactory) : IEntradasService
{
    public async Task<bool> Guardar(Entradas entrada)
    {
        if (entrada.EntradasDetalles.Count == 0 || entrada.EntradasDetalles.Any(d => d.Cantidad <= 0))
            return false;

        if (entrada.EntradaId == 0)
            return await Insertar(entrada);
        else
            return await Modificar(entrada);
    }

    private async Task<bool> Insertar(Entradas entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await using var tx = await contexto.Database.BeginTransactionAsync();

        entrada.Total = entrada.EntradasDetalles.Sum(d => d.Cantidad);
        contexto.Entradas.Add(entrada);

        foreach (var d in entrada.EntradasDetalles)
        {
            var libro = await contexto.Libros.FindAsync(d.LibroId);
            if (libro == null) return false;
            libro.CantidadDisponible += d.Cantidad;
        }

        var ok = await contexto.SaveChangesAsync() > 0;
        await tx.CommitAsync();
        return ok;
    }

    private async Task<bool> Modificar(Entradas entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await using var tx = await contexto.Database.BeginTransactionAsync();

        var original = await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == entrada.EntradaId);

        if (original == null) return false;

        foreach (var d in original.EntradasDetalles)
        {
            var libro = await contexto.Libros.FindAsync(d.LibroId);
            if (libro != null) libro.CantidadDisponible -= d.Cantidad;
        }
        contexto.EntradasDetalle.RemoveRange(original.EntradasDetalles);

        var nuevos = entrada.EntradasDetalles.Select(d => new EntradasDetalle
        {
            EntradaId = original.EntradaId,
            LibroId = d.LibroId,
            Cantidad = d.Cantidad
        }).ToList();

        foreach (var d in nuevos)
        {
            var libro = await contexto.Libros.FindAsync(d.LibroId);
            if (libro == null) return false;
            libro.CantidadDisponible += d.Cantidad;
        }
        contexto.EntradasDetalle.AddRange(nuevos);

        original.Fecha = entrada.Fecha;
        original.Concepto = entrada.Concepto;
        original.Total = nuevos.Sum(d => d.Cantidad);

        if (contexto.ChangeTracker.Entries<Libros>().Any(e => e.Entity.CantidadDisponible < 0))
            return false;

        var ok = await contexto.SaveChangesAsync() > 0;
        await tx.CommitAsync();
        return ok;
    }

    public async Task<Entradas?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await using var tx = await contexto.Database.BeginTransactionAsync();

        var entrada = await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == id);

        if (entrada == null) return false;

        foreach (var d in entrada.EntradasDetalles)
        {
            var libro = await contexto.Libros.FindAsync(d.LibroId);
            if (libro != null) libro.CantidadDisponible -= d.Cantidad;
        }

        if (contexto.ChangeTracker.Entries<Libros>().Any(e => e.Entity.CantidadDisponible < 0))
            return false;

        contexto.Entradas.Remove(entrada);
        var ok = await contexto.SaveChangesAsync() > 0;
        await tx.CommitAsync();
        return ok;
    }

    public async Task<List<Entradas>> Listar(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}