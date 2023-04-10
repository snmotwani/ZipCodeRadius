using Microsoft.EntityFrameworkCore;
using ZipCodeRadius.Model;

namespace ZipCodeRadius.Data
{
    public class BloodTypesRepo : IBloodTypesRepo
    {
        private readonly AppDBContext _dbContext;
        public BloodTypesRepo(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Blood> GetBloodTypes()
        {
            return _dbContext.BloodTypes.ToList<Blood>();
        }

        public Blood GetBloodTypeInfo(string bloodType)
        {
            var blood = _dbContext.BloodTypes.FirstOrDefault(b => b.BloodType == bloodType);
            return blood;
        }

        public double GetPopulationByBloodType(string bloodType)
        {
            double populationPct = _dbContext.BloodTypes
                .FirstOrDefault(b => b.BloodType == bloodType)
                .PopulationPct;

            return populationPct;
        }

        public double GetDonorPopulationForBloodType(string bloodType)
        {
            double donorPopulationPct = _dbContext.BloodTypes
            .FirstOrDefault(b => b.BloodType == bloodType)
            .DonorPopulationPct;

            return donorPopulationPct;
        }
    }
}
