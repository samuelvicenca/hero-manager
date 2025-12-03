namespace HeroManager.Api.Domain.Entities
{
    public class Hero
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string NomeHeroi { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public double Altura { get; set; }
        public double Peso { get; set; }

        public ICollection<HeroSuperpower> HeroSuperpowers { get; set; } = new List<HeroSuperpower>();
    }
}
