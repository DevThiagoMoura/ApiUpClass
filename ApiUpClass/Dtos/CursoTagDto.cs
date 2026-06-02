using System.ComponentModel.DataAnnotations;

namespace ApiUpClass.Dtos
{
    public class CursoTagDto
    {
        [Required]
        public int CursoId { get; set; }
        [Required]
        public int TagId { get; set; }
    }
}
