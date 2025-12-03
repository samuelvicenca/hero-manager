using HeroManager.Api.Domain.Entities;

namespace HeroManager.Api.Domain.Repositories
{
    public interface ISuperpowerRepository
    {
        Task<IReadOnlyList<Superpower>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<Superpower>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);

        Task<bool> ExistsByNameAsync(string superpoder, CancellationToken ct = default);
        Task AddAsync(Superpower superpower, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
