using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class AvaliacaoDto
    {
        [Required]
        public int CursoId { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        [Range(1, 5)]
        public decimal Nota { get; set; }
        public string? Comentario { get; set; }
    }
}