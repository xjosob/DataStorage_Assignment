using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;

public class ProjectEntity
{
    [Key]
    public string Id { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string ProjectName { get; set; } = null!;
    public string? ProjectDescription { get; set; }

    [Column(TypeName = "date")]
    [Required]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public CustomerEntity Customer { get; set; } = null!;

    [Required]
    public int StatusId { get; set; }
    public StatusTypes Status { get; set; } = null!;
    public int UserId { get; set; }

    [Required]
    public UserEntity User { get; set; } = null!;

    [Required]
    public ProductEntity Product { get; set; } = null!;
}
