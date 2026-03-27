using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public interface IReportable
    {
        string GenerateReport();
        double GetTotalRevenue();
        int GetTransactionCount();
    }
}