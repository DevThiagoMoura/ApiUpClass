using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class UsuarioDto
    {
        [Required]
        [MinLength(3)]
        public required string Nome { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Senha { get; set; }

        [Required]
        public required string Papel { get; set; }
    }
}