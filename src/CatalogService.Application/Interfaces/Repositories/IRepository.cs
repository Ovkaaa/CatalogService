﻿namespace CatalogService.Application.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
}
