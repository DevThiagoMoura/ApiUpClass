using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class CategoriaDto
    {
        [Required(ErrorMessage = "O campo Nome e obrigatorio")]
        [MinLength(3, ErrorMessage = "O nome deve ter no minimo 3 caracteres")]
        public required string Nome { get; set; }

        public string? Descricao { get; set; }
    }
}
