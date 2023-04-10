using ZipCodeRadius.Model;

namespace ZipCodeRadius.Data;
public interface IZipCodeRepo
{
    bool SaveChanges();

    ZipCodeData GetZipCodeDataByZipCode(string zipCode);

    IEnumerable<ZipCodeData> GetZipCodeInfoWithinZipCode(string zipCode, int radius);

    (int, int, IEnumerable<ZipCodeData>) GetZipCodeInfoWithZipCodePopulation(string zipCode, int population);
}
