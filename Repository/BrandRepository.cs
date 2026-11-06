using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class BrandRepository : IRepository<Brand>
{
    private StoreContext _context;

    public BrandRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task Add(Brand entity) => await _context.Brands.AddAsync(entity);

    public void Delete(Brand entity) => _context.Brands.Remove(entity);

    public async Task<IEnumerable<Brand>> Get() => 
        await _context.Brands.ToListAsync();

    public async Task<Brand> GetByTd(int Id) =>
        await _context.Brands.FindAsync(Id);

    public async Task Save() => await _context.SaveChangesAsync();

    public void Update(Brand entity)
    {
        _context.Brands.Attach(entity);
        _context.Brands.Entry(entity).State = EntityState.Modified;
    }
}