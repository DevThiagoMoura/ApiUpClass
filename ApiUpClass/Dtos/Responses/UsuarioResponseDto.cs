namespace ApiUpClass.Dtos.Responses
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public required string Email { get; set; }

        public required string Papel { get; set; }

        public DateTime CriadoEm { get; set; }
    }
}
