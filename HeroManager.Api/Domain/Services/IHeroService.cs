using HeroManager.Api.Domain.Entities;

namespace HeroManager.Api.Domain.Services
{
    public interface IHeroService
    {
        Task<Hero> CreateAsync(Hero hero, IEnumerable<int> superpowerIds, CancellationToken ct = default);
        Task<Hero> UpdateAsync(int id, Hero updatedHero, IEnumerable<int> superpowerIds, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
