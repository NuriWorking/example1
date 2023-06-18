using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question4_Server
{
    public class ReportData
    {
        public ReportData()
        {
        }

        public ReportData(string validatorId, int checkCount, int errorCount)
        {
            ValidatorId = validatorId;
            CheckCount = checkCount;
            ErrorCount = errorCount;
        }

        public string ValidatorId { get; set; }
        public int CheckCount { get; set; }
        public int ErrorCount { get; set; }
        
        public void AddCheckCount()
        {
            CheckCount++;
        }
        public void AddErrorCount()
        {
            ErrorCount++;
        }
    }
}
