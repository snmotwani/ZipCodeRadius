namespace ZipCodeRadius.DTOs;

public class zipCodeResultsDto 
{
    public string zipCode {get; set;}

    public int resultCount { get; set; }

    public int radius { get; set; }

    public int radiusPopulation { get; set; }

    public IEnumerable<zipCodeBasicInfoDto> result { get; set; }
}
