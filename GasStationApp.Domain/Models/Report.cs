using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class Report : IReportable
    {
        public List<Sale> Sales { get; private set; }
        public DateTime GeneratedAt { get; private set; }
        public string ReportType { get; private set; }

        public Report(List<Sale> sales, string reportType)
        {
            Sales = sales;
            GeneratedAt = DateTime.Now;
            ReportType = reportType;
        }

        //IReportable
        public string GenerateReport()
        {
            throw new NotImplementedException();
        }

        public double GetTotalRevenue()
        {
            throw new NotImplementedException();
        }

        public int GetTransactionCount()
        {
            throw new NotImplementedException();
        }

        //Фільтрація по користувачу (для звіту оператора)
        public List<Sale> FilterByUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}