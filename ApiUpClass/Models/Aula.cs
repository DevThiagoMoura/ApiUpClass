using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUpClass.Models
{
    [Table("aulas"), PrimaryKey(nameof(Id))]
    public class Aula
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("titulo")]
        public required string Titulo { get; set; }
        [Column("modulo_id")]
        public int ModuloId { get; set; }
        [JsonIgnore]
        [Column("duracao")]
        public int? Duracao { get; set; }
        [JsonIgnore]
        [Column("url_video")]
        public string? UrlVideo { get; set; }
        public virtual Modulo? Modulo { get; set; }
    }
}
