using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    // Alias / backward compatibility class for GymPackage
    [NotMapped]
    public class Package : GymPackage
    {
    }
}
