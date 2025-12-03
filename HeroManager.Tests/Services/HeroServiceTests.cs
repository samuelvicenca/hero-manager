using HeroManager.Api.Domain.Entities;
using HeroManager.Api.Domain.Repositories;
using HeroManager.Api.Infrastructure.Services;
using Moq;
using Xunit;

namespace HeroManager.Tests.Services
{
    public class HeroServiceTests
    {
        private readonly Mock<IHeroRepository> _heroRepo;
        private readonly Mock<ISuperpowerRepository> _superpowerRepo;
        private readonly HeroService _service;

        public HeroServiceTests()
        {
            _heroRepo = new Mock<IHeroRepository>();
            _superpowerRepo = new Mock<ISuperpowerRepository>();

            _service = new HeroService(_heroRepo.Object, _superpowerRepo.Object);
        }

        // ============================================================
        // CREATE
        // ============================================================

        [Fact]
        public async Task CreateAsync_Should_Throw_When_HeroName_AlreadyExists()
        {
            var hero = new Hero { NomeHeroi = "Batman" };

            _heroRepo
                .Setup(r => r.ExistsByNomeHeroiAsync("Batman", null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateAsync(hero, new[] { 1 }, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_No_Valid_Superpowers()
        {
            var hero = new Hero { NomeHeroi = "Batman" };

            _heroRepo
                .Setup(r => r.ExistsByNomeHeroiAsync("Batman", null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _superpowerRepo
                .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Superpower>()); // Nenhum superpoder encontrado

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateAsync(hero, new[] { 99 }, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Hero_When_Valid()
        {
            var hero = new Hero { NomeHeroi = "Batman" };

            _heroRepo
                .Setup(r => r.ExistsByNomeHeroiAsync("Batman", null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _superpowerRepo
                .Setup(r => r.GetByIdsAsync(new[] { 1 }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Superpower>
                {
                    new Superpower { Id = 1, Superpoder = "Força", Descricao = "Super força" }
                });

            _heroRepo.Setup(r => r.AddAsync(hero, It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            _heroRepo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(hero, new[] { 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Batman", result.NomeHeroi);

            // valida relação N:N
            Assert.Single(result.HeroSuperpowers);
            Assert.Equal(1, result.HeroSuperpowers.First().SuperpoderId);
        }

        // ============================================================
        // UPDATE
        // ============================================================

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Hero_NotFound()
        {
            _heroRepo.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Hero?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.UpdateAsync(10, new Hero(), new[] { 1 }, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Name_AlreadyExists()
        {
            var existing = new Hero { Id = 10, NomeHeroi = "Batman" };
            var updated = new Hero { NomeHeroi = "Superman" };

            _heroRepo.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(existing);

            _heroRepo.Setup(r => r.ExistsByNomeHeroiAsync("Superman", 10, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateAsync(10, updated, new[] { 1 }, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Superpowers()
        {
            var existing = new Hero
            {
                Id = 10,
                NomeHeroi = "Batman",
                HeroSuperpowers = new List<HeroSuperpower>
                {
                    new HeroSuperpower { HeroiId = 10, SuperpoderId = 1 }
                }
            };

            var updated = new Hero
            {
                NomeHeroi = "Batman Atualizado",
                Nome = "Bruce Wayne Atualizado"
            };

            _heroRepo.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(existing);

            _heroRepo.Setup(r =>
                r.ExistsByNomeHeroiAsync(updated.NomeHeroi, 10, It.IsAny<CancellationToken>())
            ).ReturnsAsync(false);

            _superpowerRepo
                .Setup(r => r.GetByIdsAsync(new[] { 2 }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Superpower>
                {
                    new Superpower { Id = 2, Superpoder = "Velocidade", Descricao = "Corre muito rápido" }
                });

            _heroRepo.Setup(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            _heroRepo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(10, updated, new[] { 2 }, CancellationToken.None);

            Assert.Equal("Batman Atualizado", result.NomeHeroi);
            Assert.Single(result.HeroSuperpowers);
            Assert.Equal(2, result.HeroSuperpowers.First().SuperpoderId);
        }

        // ============================================================
        // DELETE
        // ============================================================

        [Fact]
        public async Task DeleteAsync_Should_Throw_When_NotFound()
        {
            _heroRepo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Hero?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.DeleteAsync(5, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_When_Exists()
        {
            var hero = new Hero { Id = 5, NomeHeroi = "Flash" };

            _heroRepo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(hero);

            _heroRepo.Setup(r => r.DeleteAsync(hero, It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            _heroRepo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            await _service.DeleteAsync(5, CancellationToken.None);

            _heroRepo.Verify(r => r.DeleteAsync(hero, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
