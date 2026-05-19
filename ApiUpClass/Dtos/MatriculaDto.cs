using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class MatriculaDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int CursoId { get; set; }
    }
}