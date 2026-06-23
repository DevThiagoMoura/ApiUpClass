namespace ApiUpClass.Dtos.Responses
{
    public class MatriculaResponseDto
    {
        public int Id { get; set; }

        public UsuarioResponseDto? Usuario { get; set; }

        public CursoResumoResponseDto? Curso { get; set; }

        public DateTime DataMatricula { get; set; }

        public required string Status { get; set; }
    }
}
