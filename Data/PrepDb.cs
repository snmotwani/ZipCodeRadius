using Microsoft.EntityFrameworkCore;
using ZipCodeRadius.Model;

namespace ZipCodeRadius.Data;

public class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            var DbContext = serviceScope.ServiceProvider.GetService<AppDBContext>();
            if(DbContext != null)
            {
                seedData(DbContext);
            }
            else
            {
                System.Console.WriteLine("app dbContext is null - Data is NOT loaded in Cache");
            }
        }        
    }

    private static void seedData(AppDBContext dbContext)
    {
        if (!dbContext.zipCodeDatas.Any())
        {
            var lines = File.ReadAllLines(@"Data\SeedData\uszips.csv");

            // System.Console.WriteLine(lines.Count());

            var zips = lines
                .Skip(1)
                .Select(line => line.Split("\",\""))
                .Select(column => new ZipCodeData
                {
                    ZipCode = column[0].Replace("\"",""),
                    cityName = column[3],
                    state = column[5],
                    stateAbbr = column[4],
                    county = column[11],
                    latitude = double.Parse(column[1].Replace("\"","")),
                    longtitude = double.Parse(column[2].Replace("\"","")),
                    population = int.Parse(column[8]),
                    //populationDensity = double.Parse(column[9].Replace("\"",""))
                });
            
            dbContext.AddRange(zips);
            dbContext.SaveChanges();

            // System.Console.WriteLine(dbContext.zipCodeDatas.Count());
        }
        else
        {
            System.Console.WriteLine("zipCodeData is already loaded in Cache :)");
        }

        if (!dbContext.BloodTypes.Any())
        {
            var lines = File.ReadAllLines(@"Data\SeedData\bloodType.csv");

            System.Console.WriteLine(lines.Count());

            var bloodTypes = lines
                .Skip(1)
                .Select(line => line.Split("\",\""))
                .Select(column => new Blood
                {   
                    BloodType = column[0].Replace("\"",""),
                    PopulationPct =  double.Parse(column[2].Replace("\"","")),
                    DonorPopulationPct =  double.Parse(column[1].Replace("\"","")),
                    DonorBloodTypes = column[3].Replace("\"", "")
                });
            
            dbContext.AddRange(bloodTypes);
            dbContext.SaveChanges();

            // System.Console.WriteLine(dbContext.BloodTypes.Count());

            // foreach(var bloodType in dbContext.BloodTypes.ToList())
            // {
            //     System.Console.WriteLine($"{bloodType.BloodType} = {bloodType.DonorPopulation}");
            // }
        }
        else
        {
            System.Console.WriteLine("BloodTypes is already loaded in Cache :)");
        }
    }
}
