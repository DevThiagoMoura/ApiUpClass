using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class AvaliacaoUpdateDto
    {
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public int CursoId { get; set; }
        [Required]
        [Range(1, 5)]
        public decimal Nota { get; set; }
        public string? Comentario { get; set; }
    }
}
