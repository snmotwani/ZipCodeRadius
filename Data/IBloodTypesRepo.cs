using ZipCodeRadius.Model;

namespace ZipCodeRadius.Data;

public interface IBloodTypesRepo
{
    IEnumerable<Blood> GetBloodTypes();

    Blood GetBloodTypeInfo(string bloodType);

    double GetPopulationByBloodType(string bloodType);

    double GetDonorPopulationForBloodType(string bloodType);
}

