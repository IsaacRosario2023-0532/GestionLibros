using System.Linq.Expressions;

namespace GestionLibros.Shared;

public interface IServiceResult<TEntity, TId>
{
    Task<Result> Guardar(TEntity entity);
    Task<Result<TEntity?>> Buscar(TId id);
    Task<Result<List<TEntity>>> GetList(Expression<Func<TEntity, bool>> criterio);
    Task<Result> Eliminar(TId id);
}
