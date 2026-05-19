using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class UsuarioUpdateDto
    {
        [Required]
        [MinLength(3)]
        public required string Nome { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Papel { get; set; }
    }
}