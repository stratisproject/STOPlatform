using CsvHelper.Configuration;

namespace Stratis.STOPlatform.Core.CsvMappings
{
    public class UserDepositMap : ClassMap<UserDepositCsv>
    {
        public UserDepositMap()
        {
            Map(m => m.FullName);
            Map(m => m.Invested);
            Map(m => m.EarnedToken).Name("Earned Token");
            Map(m => m.Date).Name("Investment Date");
        }
    }
}