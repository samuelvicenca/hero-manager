using HeroManager.Api.Domain.Entities;
using HeroManager.Api.Domain.Repositories;
using HeroManager.Api.Domain.Services;

namespace HeroManager.Api.Infrastructure.Services
{
    public class HeroService : IHeroService
    {
        private readonly IHeroRepository _heroRepository;
        private readonly ISuperpowerRepository _superpowerRepository;

        public HeroService(IHeroRepository heroRepository, ISuperpowerRepository superpowerRepository)
        {
            _heroRepository = heroRepository;
            _superpowerRepository = superpowerRepository;
        }

        public async Task<Hero> CreateAsync(Hero hero, IEnumerable<int> superpowerIds, CancellationToken ct = default)
        {
            if (await _heroRepository.ExistsByNomeHeroiAsync(hero.NomeHeroi, null, ct))
                throw new InvalidOperationException("Já existe um super-herói com esse nome de herói.");

            var superpowers = await _superpowerRepository.GetByIdsAsync(superpowerIds, ct);
            if (!superpowers.Any())
                throw new InvalidOperationException("É obrigatório informar ao menos um superpoder válido.");

            foreach (var sp in superpowers)
            {
                hero.HeroSuperpowers.Add(new HeroSuperpower
                {
                    Heroi = hero,
                    Superpoder = sp
                });
            }

            await _heroRepository.AddAsync(hero, ct);
            await _heroRepository.SaveChangesAsync(ct);

            return hero;
        }

        public async Task<Hero> UpdateAsync(int id, Hero updatedHero, IEnumerable<int> superpowerIds, CancellationToken ct = default)
        {
            var existing = await _heroRepository.GetByIdAsync(id, ct);
            if (existing is null)
                throw new KeyNotFoundException("Super-herói não encontrado.");

            if (await _heroRepository.ExistsByNomeHeroiAsync(updatedHero.NomeHeroi, id, ct))
                throw new InvalidOperationException("Já existe outro super-herói usando esse nome de herói.");

            existing.Nome = updatedHero.Nome;
            existing.NomeHeroi = updatedHero.NomeHeroi;
            existing.DataNascimento = updatedHero.DataNascimento;
            existing.Altura = updatedHero.Altura;
            existing.Peso = updatedHero.Peso;

            existing.HeroSuperpowers.Clear();
            var superpowers = await _superpowerRepository.GetByIdsAsync(superpowerIds, ct);

            if (!superpowers.Any())
                throw new InvalidOperationException("É obrigatório informar ao menos um superpoder válido.");

            foreach (var sp in superpowers)
            {
                existing.HeroSuperpowers.Add(new HeroSuperpower
                {
                    HeroiId = existing.Id,
                    SuperpoderId = sp.Id
                });
            }

            await _heroRepository.UpdateAsync(existing, ct);
            await _heroRepository.SaveChangesAsync(ct);

            return existing;
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _heroRepository.GetByIdAsync(id, ct);
            if (existing is null)
                throw new KeyNotFoundException("Super-herói não encontrado.");

            await _heroRepository.DeleteAsync(existing, ct);
            await _heroRepository.SaveChangesAsync(ct);
        }
    }
}
