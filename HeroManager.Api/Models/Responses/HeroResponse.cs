namespace HeroManager.Api.Models.Responses
{
    public class HeroResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string NomeHeroi { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public double Altura { get; set; }
        public double Peso { get; set; }
        public List<SuperpowerResponse> Superpoderes { get; set; } = new();
    }
}
