using Status.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Status.Infrastructure.Models
{
    public class StatusOptions
    {
        public TimeSpan Rate { get; set; } = TimeSpan.FromMinutes(5);
        [Required]
        public List<Server> Servers { get; set; }
    }
}
