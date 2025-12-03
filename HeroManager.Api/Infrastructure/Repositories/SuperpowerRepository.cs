using HeroManager.Api.Domain.Entities;
using HeroManager.Api.Domain.Repositories;
using HeroManager.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HeroManager.Api.Infrastructure.Repositories
{
    public class SuperpowerRepository : ISuperpowerRepository
    {
        private readonly HeroContext _context;

        public SuperpowerRepository(HeroContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Superpower>> GetAllAsync(CancellationToken ct = default)
            => await _context.Superpoderes.AsNoTracking().ToListAsync(ct);

        public async Task<IReadOnlyList<Superpower>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
            => await _context.Superpoderes
                             .Where(s => ids.Contains(s.Id))
                             .ToListAsync(ct);

        public async Task<bool> ExistsByNameAsync(string superpoder, CancellationToken ct = default)
    => await _context.Superpoderes.AnyAsync(s => s.Superpoder == superpoder, ct);

        public async Task AddAsync(Superpower superpower, CancellationToken ct = default)
            => await _context.Superpoderes.AddAsync(superpower, ct);

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);
    }
}
