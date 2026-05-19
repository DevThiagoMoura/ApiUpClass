using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class PagamentoDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Valor { get; set; }
    }
}