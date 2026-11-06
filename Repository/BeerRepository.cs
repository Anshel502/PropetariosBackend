using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;



public class BeerRepository : IRepository<Beer>
{
private StoreContext _contex;

    public BeerRepository(StoreContext context)
    {
        _contex = context;
    }

    public async Task Add(Beer entity)=> await _contex.Beers.AddAsync(entity);

    public void Delete(Beer entity)=> _contex.Beers.Remove(entity);

    public async Task<IEnumerable<Beer>> Get()=> 
    await _contex.Beers.ToListAsync();

    public async Task<Beer> GetByTd(int Id)=>
    await _contex.Beers.FindAsync(Id);
    public async Task Save()=> await _contex.SaveChangesAsync();

    public void Update(Beer entity)
    {
        _contex.Beers.Attach(entity);
        _contex.Beers.Entry(entity).State = EntityState.Modified;
    }
}
