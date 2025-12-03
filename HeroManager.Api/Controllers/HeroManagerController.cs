using AutoMapper;
using HeroManager.Api.Domain.Entities;
using HeroManager.Api.Domain.Repositories;
using HeroManager.Api.Domain.Services;
using HeroManager.Api.Models.Requests;
using HeroManager.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HeroManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroManagerController : ControllerBase
    {
        private readonly IHeroRepository _heroRepository;
        private readonly IHeroService _heroService;
        private readonly IMapper _mapper;

        public HeroManagerController(IHeroRepository heroRepository, IHeroService heroService, IMapper mapper)
        {
            _heroRepository = heroRepository;
            _heroService = heroService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HeroResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var heroes = await _heroRepository.GetAllAsync(ct);
            if (!heroes.Any())
                return NoContent(); // nenhum herói cadastrado

            var response = _mapper.Map<IEnumerable<HeroResponse>>(heroes);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(HeroResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var hero = await _heroRepository.GetByIdAsync(id, ct);
            if (hero is null)
                return NotFound(new { message = "Super-herói não encontrado." });

            return Ok(_mapper.Map<HeroResponse>(hero));
        }

        [HttpPost("cadastrar-heroi")]
        [ProducesResponseType(typeof(HeroResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] HeroCreateRequest request, CancellationToken ct)
        {
            try
            {
                var hero = _mapper.Map<Hero>(request);
                var created = await _heroService.CreateAsync(hero, request.SuperpoderIds, ct);
                var response = _mapper.Map<HeroResponse>(created);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(HeroResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] HeroCreateRequest request, CancellationToken ct)
        {
            try
            {
                var hero = _mapper.Map<Hero>(request);
                var updated = await _heroService.UpdateAsync(id, hero, request.SuperpoderIds, ct);
                return Ok(_mapper.Map<HeroResponse>(updated));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                await _heroService.DeleteAsync(id, ct);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
