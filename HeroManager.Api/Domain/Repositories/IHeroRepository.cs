using HeroManager.Api.Domain.Entities;

namespace HeroManager.Api.Domain.Repositories
{
    public interface IHeroRepository
    {
        Task<IReadOnlyList<Hero>> GetAllAsync(CancellationToken ct = default);
        Task<Hero?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsByNomeHeroiAsync(string nomeHeroi, int? ignoreId = null, CancellationToken ct = default);
        Task AddAsync(Hero hero, CancellationToken ct = default);
        Task UpdateAsync(Hero hero, CancellationToken ct = default);
        Task DeleteAsync(Hero hero, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
