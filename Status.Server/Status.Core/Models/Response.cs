using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Status.Core.Models
{
    public class Response
    {
        [Required]
        public TimeSpan ResponseTime { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
        public string? ReasonPhrase { get; set; }
    }
}