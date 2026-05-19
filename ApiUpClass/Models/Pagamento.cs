using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiUpClass.Models
{
    [Table("pagamentos"), PrimaryKey(nameof(Id))]
    public class Pagamento
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public virtual Usuario? Usuario { get; set; }

        [Column("curso_id")]
        public int CursoId { get; set; }

        public virtual Curso? Curso { get; set; }

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("status")]
        public required string Status { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }
    }
}