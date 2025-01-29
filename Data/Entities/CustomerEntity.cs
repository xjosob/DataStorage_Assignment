using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;

namespace Data.Entities
{
    public class CustomerEntity
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
    }
}

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ProjectName { get; set; } = null!;
    public string? ProjectDescription { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }
    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;
    public int StatusId { get; set; }
    public StatusTypeEntity Status { get; set; } = null!;
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public ProductEntity Product { get; set; } = null!;
}
