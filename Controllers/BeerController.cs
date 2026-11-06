using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private IValidator<BeerInsertDto> _beerInsertValidator;
        private IValidator<BeerUpdateDto> _beerUpdateValidator;
        private ICommonService<BeerDto, BeerInsertDto, BeerUpdateDto> _beerService;
        public BeerController(
            IValidator<BeerInsertDto> beerInsertValidator,
            IValidator<BeerUpdateDto> beerUpdateValidator,
            [FromKeyedServices("beerService")] ICommonService<BeerDto, BeerInsertDto, BeerUpdateDto> beerService)
        {
            _beerInsertValidator = beerInsertValidator;
            _beerUpdateValidator = beerUpdateValidator;
            _beerService = beerService;
        }
        [HttpGet]
        public async Task<IEnumerable<BeerDto>> Get() =>
        await _beerService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDto>> GetById(int id)
        {
            var beer = await _beerService.GetById(id);
            return beer == null ? NotFound() : Ok(beer);
        }

        [HttpPost]
        public async Task<ActionResult<BeerDto>> Add(BeerInsertDto beerInsertDto)
        {
            var validationResult = await _beerInsertValidator.ValidateAsync(beerInsertDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var beerDto = await _beerService.Add(beerInsertDto);

            return CreatedAtAction(nameof(GetById), new { id = beerDto.Id }, beerDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BeerDto>> Update(int id, BeerUpdateDto beerUpdateDto)
        {
            var validationResult = await _beerUpdateValidator.ValidateAsync(beerUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var beer = await _beerService.Update(id, beerUpdateDto);
            return beer == null ? NotFound() : Ok(beer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BeerDto>> Delete(int id)
        {
            var beer = await _beerService.Delete(id);
            if (beer == null)
            {
                return NotFound();
            }
            return Ok(beer);
        }

    }
}
