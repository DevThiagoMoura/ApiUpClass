using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class CursoTagsDto
    {
        [Required(ErrorMessage = "Obrigatorio o envio de pelo menos uma tag")]
        public required List<int> Ids { get; set; }
    }
}
