using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class CustomerEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CustomerName { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string CustomerEmail { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(15)")]
        public string CustomerPhone { get; set; } = null!;
    }
}
