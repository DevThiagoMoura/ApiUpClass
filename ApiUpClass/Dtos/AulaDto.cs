using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class AulaDto
    {
        [Required]
        [MinLength(3)]
        public required string Titulo { get; set; }

        [Required]
        public int ModuloId { get; set; }

        [Required]
        public int? Duracao { get; set; }

        [Required]
        public string? UrlVideo { get; set; }
    }
}
