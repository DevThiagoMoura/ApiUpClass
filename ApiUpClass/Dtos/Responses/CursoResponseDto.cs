namespace ApiUpClass.Dtos.Responses
{
    public class CursoResponseDto
    {
        public int Id { get; set; }

        public required string Titulo { get; set; }

        public string? Descricao { get; set; }

        public decimal Preco { get; set; }

        public bool Ativo { get; set; }

        public DateTime CriadoEm { get; set; }

        public CategoriaResponseDto? Categoria { get; set; }

        public ICollection<TagResponseDto> Tags { get; set; } = new List<TagResponseDto>();
    }
}
