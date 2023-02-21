using System.ComponentModel.DataAnnotations;
using Status.Core.Models;

namespace Status.Api.Models;

public class ServersOptions
{
    public const string configurationSectionName = "Servers";
    [Required]
    public List<Server> Servers { get; set; }
}