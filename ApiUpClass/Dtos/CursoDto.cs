using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class CursoDto
    {
        [Required(ErrorMessage = "O campo Titulo e obrigatorio")]
        [MinLength(3, ErrorMessage = "O titulo deve ter no minimo 3 caracteres")]
        public required string Titulo { get; set; }

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo Preco e obrigatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "O preco nao pode ser negativo")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo Categoria e obrigatorio")]
        public int CategoriaId { get; set; }
    }
}
