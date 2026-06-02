using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class TagDto
    {
        [Required]
        [MinLength(2)]
        public required string Nome { get; set; }
    }
}
