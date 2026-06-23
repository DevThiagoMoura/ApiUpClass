namespace ApiUpClass.Dtos.Responses
{
    public class CursoResumoResponseDto
    {
        public int Id { get; set; }

        public required string Titulo { get; set; }

        public decimal Preco { get; set; }

        public bool Ativo { get; set; }
    }
}
