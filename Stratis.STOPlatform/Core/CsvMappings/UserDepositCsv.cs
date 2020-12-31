using System;

namespace Stratis.STOPlatform.Core.CsvMappings
{
    public class UserDepositCsv
    {
        public string FullName { get; set; }
        public string EarnedToken { get; set; }
        public string Invested { get; set; }
        public DateTime Date { get; set; }
    }
}