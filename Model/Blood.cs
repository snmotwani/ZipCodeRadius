using System.ComponentModel.DataAnnotations;

namespace ZipCodeRadius.Model;

public class Blood
{
    [Key, Required]
    public string BloodType { get; set; } = string.Empty;  

    // Population percentage that can donate (recive) to this blood type
    [Required]
    public double DonorPopulationPct { get; set; }

    // Percentage of population that has this blood type
    [Required]
    public double PopulationPct { get; set; }

    [Required]
    public string DonorBloodTypes { get; set; }
}
