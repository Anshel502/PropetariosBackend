using System;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services;

public class BrandService : ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto>
{
    private IRepository<Brand> _brandRepository;
    private IRepository<Beer> _beerRepository;

    public BrandService(IRepository<Brand> brandRepository, IRepository<Beer> beerRepository)
    {
        _brandRepository = brandRepository;
        _beerRepository = beerRepository;
    }

    public async Task<IEnumerable<BrandDto>> Get()
    {
        var brands = await _brandRepository.Get();
        return brands.Select(b => new BrandDto
        {
            Id = b.BrandId, 
            Name = b.Name
        });
    }

    public async Task<BrandDto> GetById(int id)
    {
        var brand = await _brandRepository.GetByTd(id);
        if (brand != null)
        {
            var brandDto = new BrandDto
            {
                Id = brand.BrandId, 
                Name = brand.Name
            };
            return brandDto;
        }
        return null;
    }

    public async Task<BrandDto> Add(BrandInsertDto brandInsertDto)
    {
        var allBrands = await _brandRepository.Get();
        if (allBrands.Any(b => b.Name.ToLower() == brandInsertDto.Name.ToLower()))
        {
            throw new InvalidOperationException("Ya existe una marca con ese nombre.");
        }

        var brand = new Brand()
        {
            Name = brandInsertDto.Name
        };
        
        await _brandRepository.Add(brand);
        await _brandRepository.Save();
        
        var brandDto = new BrandDto()
        {
            Id = brand.BrandId, 
            Name = brand.Name
        };
        return brandDto;
    }

    public async Task<BrandDto> Update(int id, BrandUpdateDto brandUpdateDto)
    {
        var brand = await _brandRepository.GetByTd(id);
        if (brand != null)
        {
            var allBrands = await _brandRepository.Get();
            if (allBrands.Any(b => b.Name.ToLower() == brandUpdateDto.Name.ToLower() && b.BrandId != id)) 
            {
                throw new InvalidOperationException("Ya existe otra marca con ese nombre.");
            }

            brand.Name = brandUpdateDto.Name;
            
            _brandRepository.Update(brand);
            await _brandRepository.Save();
            
            var brandDto = new BrandDto()
            {
                Id = brand.BrandId, 
                Name = brand.Name
            };
            return brandDto;
        }

        return null;
    }

    public async Task<BrandDto> Delete(int id)
    {
        var brand = await _brandRepository.GetByTd(id);
        if (brand != null)
        {
            var allBeers = await _beerRepository.Get();
            if (allBeers.Any(b => b.BrandId == id))
            {
                throw new InvalidOperationException("Conflict: No se puede eliminar la marca porque tiene cervezas asociadas.");
            }

            _brandRepository.Delete(brand);
            await _brandRepository.Save();
            
            var brandDto = new BrandDto
            {
                Id = brand.BrandId, 
                Name = brand.Name
            };
            return brandDto;
        }

        return null;
    }
}