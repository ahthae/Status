using System.ComponentModel.DataAnnotations;

namespace Status.Core.Models
{
    public class Server
    {
        [Required]
        [Url]
        public Uri Url { get; set; }
        public List<Response> Responses { get; set; } = new List<Response>();
    }
}