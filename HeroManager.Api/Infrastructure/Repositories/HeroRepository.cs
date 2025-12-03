using HeroManager.Api.Domain.Entities;
using HeroManager.Api.Domain.Repositories;
using HeroManager.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HeroManager.Api.Infrastructure.Repositories
{
    public class HeroRepository : IHeroRepository
    {
        private readonly HeroContext _context;

        public HeroRepository(HeroContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Hero>> GetAllAsync(CancellationToken ct = default)
            => await _context.Herois
                .Include(h => h.HeroSuperpowers)
                    .ThenInclude(hs => hs.Superpoder)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<Hero?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _context.Herois
                .Include(h => h.HeroSuperpowers)
                    .ThenInclude(hs => hs.Superpoder)
                .FirstOrDefaultAsync(h => h.Id == id, ct);

        public Task<bool> ExistsByNomeHeroiAsync(string nomeHeroi, int? ignoreId = null, CancellationToken ct = default)
        {
            IQueryable<Hero> query = _context.Herois;
            if (ignoreId.HasValue)
                query = query.Where(h => h.Id != ignoreId.Value);

            return query.AnyAsync(h => h.NomeHeroi == nomeHeroi, ct);
        }

        public async Task AddAsync(Hero hero, CancellationToken ct = default)
            => await _context.Herois.AddAsync(hero, ct);

        public Task UpdateAsync(Hero hero, CancellationToken ct = default)
        {
            _context.Herois.Update(hero);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Hero hero, CancellationToken ct = default)
        {
            _context.Herois.Remove(hero);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);
    }

}
