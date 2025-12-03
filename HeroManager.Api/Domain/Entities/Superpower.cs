namespace HeroManager.Api.Domain.Entities
{
    public class Superpower
    {
        public int Id { get; set; }
        public string Superpoder { get; set; } = null!;
        public string Descricao { get; set; } = null!;

        public ICollection<HeroSuperpower> HeroSuperpowers { get; set; } = new List<HeroSuperpower>();
    }
}
