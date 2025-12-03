namespace HeroManager.Api.Domain.Entities
{
    public class HeroSuperpower
    {
        public int HeroiId { get; set; }
        public Hero Heroi { get; set; } = null!;

        public int SuperpoderId { get; set; }
        public Superpower Superpoder { get; set; } = null!;
    }
}
