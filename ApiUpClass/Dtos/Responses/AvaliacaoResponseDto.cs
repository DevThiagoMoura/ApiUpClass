namespace ApiUpClass.Dtos.Responses
{
    public class AvaliacaoResponseDto
    {
        public int Id { get; set; }

        public UsuarioResponseDto? Usuario { get; set; }

        public CursoResumoResponseDto? Curso { get; set; }

        public decimal Nota { get; set; }

        public string? Comentario { get; set; }

        public DateTime CriadoEm { get; set; }
    }
}
