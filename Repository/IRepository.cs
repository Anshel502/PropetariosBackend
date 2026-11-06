using System;

namespace Backend.Repository;

public interface IRepository<TEntity>
{
    Task<IEnumerable<TEntity>> Get();
    Task<TEntity> GetByTd(int Id);
    Task Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);

    Task Save();
}
