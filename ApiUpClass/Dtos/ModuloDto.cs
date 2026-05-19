using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class ModuloDto
    {
        [Required]
        [MinLength(3)]
        public required string Titulo { get; set; }

        [Required]
        public int Ordem { get; set; }

        [Required]
        public int CursoId { get; set; }
    }
}
