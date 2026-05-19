using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUpClass.Models
{
    [Table("cursos"), PrimaryKey(nameof(Id))]
    public class Curso
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public required string Titulo { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("preco")]
        public decimal Preco { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        [JsonIgnore]
        [Column("categoria_id")]
        public int CategoriaId { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public required ICollection<Modulo> Modulos { get; set; } 
    }
}
