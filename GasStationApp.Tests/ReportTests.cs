using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class ReportTests
    {
        private List<Sale> CreateSales()
        {
            var fuel = new FuelType("А-95", 56.50);
            var op = new Operator("op1", "pass");
            return new List<Sale>
            {
                new Sale(fuel, 10, op, null),
                new Sale(fuel, 20, op, null)
            };
        }

        [Fact]
        public void GetTotalRevenue_ReturnsCorrectSum()
        {
            var report = new Report(CreateSales(), "Financial");
            Assert.Equal(1695.0, report.GetTotalRevenue(), 2);
        }

        [Fact]
        public void GetTransactionCount_ReturnsCorrectCount()
        {
            var report = new Report(CreateSales(), "Financial");
            Assert.Equal(2, report.GetTransactionCount());
        }

        [Fact]
        public void FilterByUser_ReturnsOnlySalesByThatUser()
        {
            var fuel = new FuelType("А-95", 56.50);
            var op1 = new Operator("op1", "pass");
            var op2 = new Operator("op2", "pass");
            var sales = new List<Sale>
            {
                new Sale(fuel, 10, op1, null),
                new Sale(fuel, 5, op2, null),
                new Sale(fuel, 8, op1, null)
            };
            var report = new Report(sales, "Shift");
            var filtered = report.FilterByUser(op1);
            Assert.Equal(2, filtered.Count);
        }

        [Fact]
        public void GenerateReport_ReturnsNonEmptyString()
        {
            var report = new Report(CreateSales(), "Financial");
            var result = report.GenerateReport();
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}