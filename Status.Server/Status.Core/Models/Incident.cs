using System.ComponentModel.DataAnnotations;

namespace Status.Core.Models
{
    public class Incident
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}