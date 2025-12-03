namespace HeroManager.Api.Models.Requests
{
    public class HeroCreateRequest
    {
        public string Nome { get; set; } = null!;
        public string NomeHeroi { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public double Altura { get; set; }
        public double Peso { get; set; }
        public List<int> SuperpoderIds { get; set; } = new();
    }
}
