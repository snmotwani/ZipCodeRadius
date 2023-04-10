using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZipCodeRadius.Model;

public class ZipCodeData
{
    [Key]
    public int id {get; set;}

    [Required]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    public double latitude {get; set;}

    [Required]
    public double longtitude { get; set; }

    [Required]
    public string cityName { get; set; } = string.Empty;

    public string county {get; set;} = string.Empty;

    [Required]
    public string stateAbbr {get; set;} = string.Empty;

    public string state {get; set;} = string.Empty;

    [DefaultValue(0)]
    public int population { get; set; }

    [DefaultValue(0)]
    public double populationDensity {get; set;}
}
