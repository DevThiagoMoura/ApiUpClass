namespace ApiUpClass.Dtos.Responses
{
    public class AulaResponseDto
    {
        public int Id { get; set; }

        public required string Titulo { get; set; }

        public int? Duracao { get; set; }

        public string? UrlVideo { get; set; }

        public ModuloResumoResponseDto? Modulo { get; set; }
    }
}
