using Microsoft.Extensions.Configuration;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class AFINNLemmasRater : ILemmasRater
    {
        private readonly IConfiguration _configuration;

        public AFINNLemmasRater(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Dictionary<Guid, double> RateLemmas(Dictionary<Guid, string[]> lemmasById)
        {
            var lemmaRateById = GetRateLemmaPairDictionary();
            var rateById = new Dictionary<Guid, double>();
            foreach (var lemmasIdPair in lemmasById)
            {
                int ratedLemmasCount = 0;
                int rateSum = 0;
                foreach (var lemmas in lemmasIdPair.Value)
                {
                    if(lemmaRateById.TryGetValue(lemmas, out int? rate) && rate.HasValue)
                    {
                        ratedLemmasCount++;
                        rateSum += (int)rate;
                    }
                }
                if (ratedLemmasCount != 0)
                    rateById.Add(lemmasIdPair.Key, (double)rateSum / (double)ratedLemmasCount);
            }
            return rateById;
        }

        private Dictionary<string, int?> GetRateLemmaPairDictionary()
        {
            var afinnConfiguration = _configuration.GetSection("AFINN");
            if (afinnConfiguration is null || !afinnConfiguration.GetChildren().Any())
                throw new Exception("There is no AFINN in the configuration");
            return afinnConfiguration.GetChildren().ToDictionary(s => s.Key,
                s => Int32.TryParse(s.Value, out int value) ? value : default(int?));
        }
    }
}
