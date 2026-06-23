namespace ApiUpClass.Dtos.Responses
{
    public class ModuloResponseDto
    {
        public int Id { get; set; }

        public required string Titulo { get; set; }

        public int Ordem { get; set; }

        public CursoResumoResponseDto? Curso { get; set; }
    }
}
