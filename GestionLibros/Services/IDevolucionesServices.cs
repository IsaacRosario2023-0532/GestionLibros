using GestionLibros.Models;

namespace GestionLibros.Services;

public interface IDevolucionesService
{
    Task<List<Prestamos>> ListarPendientes();
    Task<bool> Devolver(int prestamoId, DateTime fechaDevolucion);
    Task<List<LibroPromedioDto>> ListarLibrosConPromedio();
}