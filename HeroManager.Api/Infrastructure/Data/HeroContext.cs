using HeroManager.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeroManager.Api.Infrastructure.Data
{
    public class HeroContext : DbContext
    {
        public HeroContext(DbContextOptions<HeroContext> options) : base(options) { }

        public DbSet<Hero> Herois => Set<Hero>();
        public DbSet<Superpower> Superpoderes => Set<Superpower>();
        public DbSet<HeroSuperpower> HeroisSuperpoderes => Set<HeroSuperpower>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hero>(static builder =>
            {
               builder.ToTable("Herois");
                builder.HasKey(h => h.Id);
                builder.Property(h => h.Nome).HasMaxLength(120).IsRequired();
                builder.Property(h => h.NomeHeroi).HasMaxLength(120).IsRequired();
                builder.Property(h => h.DataNascimento).IsRequired();
                builder.Property(h => h.Altura).IsRequired();
                builder.Property(h => h.Peso).IsRequired();
                builder.HasIndex(h => h.NomeHeroi).IsUnique();
            });

            modelBuilder.Entity<Superpower>(builder =>
            {
                builder.ToTable("Superpoderes");
                builder.HasKey(s => s.Id);
                builder.Property(s => s.Superpoder).HasMaxLength(50).IsRequired();
                builder.Property(s => s.Descricao).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<HeroSuperpower>(builder =>
            {
               builder.ToTable("HeroisSuperpoderes");
                builder.HasKey(hs => new { hs.HeroiId, hs.SuperpoderId });

                builder.HasOne(hs => hs.Heroi)
                       .WithMany(h => h.HeroSuperpowers)
                       .HasForeignKey(hs => hs.HeroiId);

                builder.HasOne(hs => hs.Superpoder)
                       .WithMany(s => s.HeroSuperpowers)
                       .HasForeignKey(hs => hs.SuperpoderId);
            });
        }
    }
}
