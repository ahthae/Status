using System.ComponentModel.DataAnnotations;

namespace Status.Core.Models
{
    public class Server
    {
        [Required]
        
        [Url]
        public Uri Url { get; set; }
        public TimeSpan Rate { get; set; } = TimeSpan.FromMinutes(5);
        public List<Response> Responses { get; set; } = new List<Response>();
    }
}