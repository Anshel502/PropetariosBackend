using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private IValidator<BrandInsertDto> _brandInsertValidator;
        private IValidator<BrandUpdateDto> _brandUpdateValidator;
        private ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto> _brandService;
        private IRepository<Beer> _beerRepository;

        public BrandController(
            IValidator<BrandInsertDto> brandInsertValidator,
            IValidator<BrandUpdateDto> brandUpdateValidator,
            [FromKeyedServices("brandService")] ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto> brandService,
            IRepository<Beer> beerRepository)
        {
            _brandInsertValidator = brandInsertValidator;
            _brandUpdateValidator = brandUpdateValidator;
            _brandService = brandService;
            _beerRepository = beerRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<BrandDto>> Get() => 
            await _brandService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetById(int id)
        {
            var brand = await _brandService.GetById(id);
            return brand == null ? NotFound() : Ok(brand);
        }

        [HttpGet("{id}/beers")]
        public async Task<ActionResult<IEnumerable<BeerDto>>> GetBeersByBrand(int id)
        {
            var brand = await _brandService.GetById(id);
            if (brand == null) return NotFound("La marca no existe.");

            var allBeers = await _beerRepository.Get();
            var brandBeers = allBeers.Where(b => b.BrandId == id).Select(b => new BeerDto
            {
                Id = b.BeerId,
                Name = b.Name,
                Alcohol = b.Alcohol,
                BrandId = b.BrandId
            });

            return Ok(brandBeers);
        }

        [HttpPost]
        public async Task<ActionResult<BrandDto>> Add(BrandInsertDto brandInsertDto)
        {
            var validationResult = await _brandInsertValidator.ValidateAsync(brandInsertDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var brandDto = await _brandService.Add(brandInsertDto);
                return CreatedAtAction(nameof(GetById), new { id = brandDto.Id }, brandDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BrandDto>> Update(int id, BrandUpdateDto brandUpdateDto)
        {
            var validationResult = await _brandUpdateValidator.ValidateAsync(brandUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var brand = await _brandService.Update(id, brandUpdateDto);
                return brand == null ? NotFound() : Ok(brand);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BrandDto>> Delete(int id)
        {
            try
            {
                var brand = await _brandService.Delete(id);
                if (brand == null)
                {
                    return NotFound();
                }
                return Ok(brand);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}