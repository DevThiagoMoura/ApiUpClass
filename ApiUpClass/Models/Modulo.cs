using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUpClass.Models
{
    [Table("modulos"), PrimaryKey(nameof(Id))]
    public class Modulo
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("titulo")]
        public required string Titulo { get; set; }

        [Column("ordem")]
        public int Ordem { get; set; }

        [Column("curso_id")]
        public int cursoId { get; set; }
        [JsonIgnore]
        public virtual Curso? curso { get; set; }

        public required ICollection<Aula> aulas { get; set; }
    }
}
