using ZipCodeRadius.Model;

namespace ZipCodeRadius.Data;

public class ZipCodeRepo : IZipCodeRepo
{
    private readonly AppDBContext _dbContext;
    private readonly ILogger _logger;

    public ZipCodeRepo(AppDBContext dbContext)
    {
        _dbContext = dbContext;
        //_logger = logger;
    }

    bool IZipCodeRepo.SaveChanges()
    {
        return (_dbContext.SaveChanges() >= 0);
    }

    public ZipCodeData GetZipCodeDataByZipCode(string zipCode)
    {
        //_logger.LogDebug($"repo call for {zipCode}");

        return _dbContext.zipCodeDatas.FirstOrDefault(p => p.ZipCode == zipCode);
    }

    public IEnumerable<ZipCodeData> GetZipCodeInfoWithinZipCode(string zipCode, int radius)
    {   
        double equatorCircumference = 24855;      //miles
        // double earthRadius = 3959;          //miles

        var currentLocation = _dbContext.zipCodeDatas.FirstOrDefault(p => p.ZipCode == zipCode);

        if(currentLocation != null) {
            double topLatitude = currentLocation.latitude + ((radius*360)/equatorCircumference) ;
            double bottomLatitude = currentLocation.latitude - ((radius*360)/equatorCircumference);

            // TO-DO : Somehow the calculations are not working - will need to check the Math
            // At the equator each 1 degree is 70 miles
            // As you go North (or south) to get 70 miles you will need more degrees (other than extreme near the poles)
            // double lefLongtitude = currentLocation.longtitude - ((radius*360)/(equatorCircumference * Math.Cos(currentLocation.latitude))) ;
            // double rightLongtitude = currentLocation.longtitude + ((radius*360)/(equatorCircumference * Math.Cos(currentLocation.latitude)));

            double lefLongtitude = currentLocation.longtitude - ((radius*360)/(equatorCircumference)) ;
            double rightLongtitude = currentLocation.longtitude + ((radius*360)/(equatorCircumference));

            // _logger.LogDebug(topLatitude);
            // _logger.LogDebug(bottomLatitude);
            // _logger.LogDebug(lefLongtitude);
            // _logger.LogDebug(rightLongtitude);

            return _dbContext.zipCodeDatas
                .Where(p => p.latitude <= topLatitude 
                    && p.latitude >= bottomLatitude 
                    && p.longtitude >= lefLongtitude 
                    && p.longtitude <= rightLongtitude)
                .ToList();
        }
        else
        {
            return null;
        }        
    }

    public (int, int, IEnumerable<ZipCodeData>) GetZipCodeInfoWithZipCodePopulation(string zipCode, int population)
    {   
        double earthCircumference = 24855;      //miles
        // double earthRadius = 3959;           //miles

        var currentLocation = _dbContext.zipCodeDatas.FirstOrDefault(p => p.ZipCode == zipCode);

        // if the current location has the given population, then no need to search further.
        if(currentLocation != null && currentLocation.population >= population)
        {
            return (0, 
                    currentLocation.population, 
                    _dbContext.zipCodeDatas
                        .Where(p => p.ZipCode == zipCode)
                        .ToList()
                    );
        }

        if(currentLocation != null) 
        {
            double degreePerMile = 360/earthCircumference;
            int limitSearchMiles = 200;     
            int radius = 0;                 // start at 1 mile radius with increment of 1 mile
            int searchedPopulation = currentLocation.population;

            double topLatitude = currentLocation.latitude;
            double bottomLatitude = currentLocation.latitude;
            double lefLongtitude = currentLocation.longtitude;
            double rightLongtitude = currentLocation.longtitude;
            
            do 
            {
                topLatitude = currentLocation.latitude + (++radius*degreePerMile) ;
                bottomLatitude = currentLocation.latitude - (radius*degreePerMile);
                lefLongtitude = currentLocation.longtitude - (radius*degreePerMile) ;
                rightLongtitude = currentLocation.longtitude + (radius*degreePerMile);

                // _logger.LogDebug($"Radius: {radius}");
                // _logger.LogDebug($"topLatitude: {topLatitude}");
                // _logger.LogDebug($"bottomLatitude: {bottomLatitude}");
                // _logger.LogDebug($"lefLongtitude: {lefLongtitude}");
                // _logger.LogDebug($"rightLongtitude: {rightLongtitude}");

                searchedPopulation = _dbContext.zipCodeDatas
                    .Where(p => p.latitude <= topLatitude 
                        && p.latitude >= bottomLatitude 
                        && p.longtitude >= lefLongtitude 
                        && p.longtitude <= rightLongtitude)
                    .Sum(p => p.population);
                //_logger.LogDebug($"Population found: {searchedPopulation}");
            }
            while(searchedPopulation < population && radius < limitSearchMiles);

            return (
                radius, 
                searchedPopulation,
                _dbContext.zipCodeDatas
                    .Where(p => p.latitude <= topLatitude 
                        && p.latitude >= bottomLatitude 
                        && p.longtitude >= lefLongtitude 
                        && p.longtitude <= rightLongtitude)
                    .ToList()
            );
        }

        // Current Zip Code not found, so no reference point to use for continued search
        return (0, 0, null);
    }
}
