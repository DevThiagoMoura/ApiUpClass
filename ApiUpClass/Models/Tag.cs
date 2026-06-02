using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUpClass.Models
{
    [Table("Tags"), PrimaryKey(nameof(Id))]
    public class Tag
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nome")]
        public required string Nome { get; set; }

        [JsonIgnore]
        public ICollection<CursoTag>? CursosTags { get; set; }
    }
}
