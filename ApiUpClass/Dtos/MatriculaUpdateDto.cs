using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class MatriculaUpdateDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}