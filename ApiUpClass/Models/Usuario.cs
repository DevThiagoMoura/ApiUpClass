using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUpClass.Models
{
    [Table("usuarios"), PrimaryKey(nameof(Id))]
    public class Usuario
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public required string Nome { get; set; }

        [Column("email")]
        public required string Email { get; set; }

        [Column("senha_hash")]
        public required string SenhaHash { get; set; }

        [Column("papel")]
        public required string Papel { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        [JsonIgnore]
        public ICollection<Matricula>? Matriculas { get; set; }

        [JsonIgnore]
        public ICollection<Pagamento>? Pagamentos { get; set; }

        [JsonIgnore]
        public ICollection<Avaliacao>? Avaliacoes { get; set; }
    }
}