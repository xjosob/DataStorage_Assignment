using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class StatusTypes
    {
        [Key]
        public int Id { get; set; }
        public string StatusName { get; set; } = null!;
    }
}
