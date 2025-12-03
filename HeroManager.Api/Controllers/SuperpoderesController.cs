using AutoMapper;
using HeroManager.Api.Domain.Entities;
using HeroManager.Api.Domain.Repositories;
using HeroManager.Api.Models.Requests;
using HeroManager.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HeroManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperpoderesController : ControllerBase
    {
        private readonly ISuperpowerRepository _superpowerRepository;
        private readonly IMapper _mapper;

        public SuperpoderesController(ISuperpowerRepository superpowerRepository, IMapper mapper)
        {
            _superpowerRepository = superpowerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SuperpowerResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var list = await _superpowerRepository.GetAllAsync(ct);
            return Ok(_mapper.Map<IEnumerable<SuperpowerResponse>>(list));
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuperpowerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] SuperpowerCreateRequest request, CancellationToken ct)
        {
            // regra simples pra evitar superpoder duplicado
            if (await _superpowerRepository.ExistsByNameAsync(request.Superpoder, ct))
                return Conflict(new { message = "Já existe um superpoder com esse nome." });

            var entity = new Superpower
            {
                Superpoder = request.Superpoder,
                Descricao = request.Descricao
            };

            await _superpowerRepository.AddAsync(entity, ct);
            await _superpowerRepository.SaveChangesAsync(ct);

            var response = _mapper.Map<SuperpowerResponse>(entity);

            return CreatedAtAction(nameof(GetAll), new { id = response.Id }, response);
        }
    }
}
