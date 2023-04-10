namespace ZipCodeRadius.DTOs;

public class zipCodeInfoDto 
{      
    public string ZipCode { get; set; } = string.Empty;
    // public double latitude {get; set;}
    // public double longtitude { get; set; }
    public string cityName { get; set; } = string.Empty;
    //public string county {get; set;} = string.Empty;
    public string stateAbbr {get; set;} = string.Empty;
    // public string state {get; set;} = string.Empty;
    public int population { get; set; }
}