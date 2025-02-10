using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class Project
    {
        public string Id { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string? ProjectDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public int StatusId { get; set; }
        public StatusType Status { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
