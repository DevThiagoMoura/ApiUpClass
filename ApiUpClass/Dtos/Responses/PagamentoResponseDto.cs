namespace ApiUpClass.Dtos.Responses
{
    public class PagamentoResponseDto
    {
        public int Id { get; set; }

        public UsuarioResponseDto? Usuario { get; set; }

        public CursoResumoResponseDto? Curso { get; set; }

        public decimal Valor { get; set; }

        public required string Status { get; set; }

        public DateTime CriadoEm { get; set; }
    }
}
