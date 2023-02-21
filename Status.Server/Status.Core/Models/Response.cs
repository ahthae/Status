using System.ComponentModel.DataAnnotations;

namespace Status.Core.Models
{
    public class Response
    {
        [Required]
        public TimeSpan ResponseTime { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
    }
}