using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiUpClass.Models
{
    [Table("cursos_tags"), PrimaryKey(nameof(CursoId), nameof(TagId))]
    public class CursoTag
    {
        [Column("curso_id")]
        public int CursoId { get; set; }

        public virtual Curso? Curso { get; set; }

        [Column("tag_id")]
        public int TagId { get; set; }

        public virtual Tag? Tag { get; set; }
    }
}
