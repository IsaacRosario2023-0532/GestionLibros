using System.Linq.Expressions;
using GestionLibros.Models;

namespace GestionLibros.Services;

public interface IEntradasService
{
    Task<bool> Guardar(Entradas entrada);
    Task<Entradas?> Buscar(int id);
    Task<bool> Eliminar(int id);
    Task<List<Entradas>> Listar(Expression<Func<Entradas, bool>> criterio);
}