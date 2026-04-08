using System;
using System.Collections.Generic;
using System.Globalization;
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
            return $"Звіт [{ReportType}] від {GeneratedAt:dd.MM.yyyy HH:mm}\n" +
                   $"Кількість транзакцій: {GetTransactionCount()}\n" +
                   $"Загальна виручка: {GetTotalRevenue().ToString("F2", CultureInfo.InvariantCulture)} грн";
        }

        public double GetTotalRevenue()
        {
            return Sales.Sum(s => s.TotalAmount);
        }

        public int GetTransactionCount()
        {
            return Sales.Count;
        }

        //Фільтрація по користувачу (для звіту оператора)
        public List<Sale> FilterByUser(User user)
        {
            return Sales.Where(s => s.PerformedBy == user).ToList();
        }
    }
}