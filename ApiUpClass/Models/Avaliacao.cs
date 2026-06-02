using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiUpClass.Models
{
    [Table("avaliacoes"), PrimaryKey(nameof(Id))]
    public class Avaliacao
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public virtual Usuario? Usuario { get; set; }

        [Column("curso_id")]
        public int CursoId { get; set; }

        public virtual Curso? Curso { get; set; }

        [Column("nota")]
        public decimal Nota { get; set; }

        [Column("comentario")]
        public string? Comentario { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }
    }
}
