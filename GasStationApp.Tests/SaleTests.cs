using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class SaleTests
    {
        private (FuelType, Administrator) CreateBasics()
        {
            var fuel = new FuelType("А-95", 56.50);
            var user = new Administrator("admin", "Admin123");
            return (fuel, user);
        }

        [Fact]
        public void Constructor_CalculatesTotalCorrectly()
        {
            var (fuel, user) = CreateBasics();
            var sale = new Sale(fuel, 10, user, null);
            Assert.Equal(565.0, sale.TotalAmount);
        }

        [Fact]
        public void CalculateBonuses_Bronze_Returns3Percent()
        {
            var (fuel, user) = CreateBasics();
            var sale = new Sale(fuel, 10, user, null);
            var bonuses = sale.CalculateBonuses("Bronze");
            Assert.Equal(565.0 * 0.03, bonuses, 2);
        }

        [Fact]
        public void CalculateBonuses_Silver_Returns5Percent()
        {
            var (fuel, user) = CreateBasics();
            var sale = new Sale(fuel, 10, user, null);
            var bonuses = sale.CalculateBonuses("Silver");
            Assert.Equal(565.0 * 0.05, bonuses, 2);
        }

        [Fact]
        public void CalculateBonuses_Gold_Returns7Percent()
        {
            var (fuel, user) = CreateBasics();
            var sale = new Sale(fuel, 10, user, null);
            var bonuses = sale.CalculateBonuses("Gold");
            Assert.Equal(565.0 * 0.07, bonuses, 2);
        }

        [Fact]
        public void CalculateBonuses_NoCard_ReturnsZero()
        {
            var (fuel, user) = CreateBasics();
            var sale = new Sale(fuel, 10, user, null);
            var bonuses = sale.CalculateBonuses(null);
            Assert.Equal(0, bonuses);
        }

        [Fact]
        public void Id_AutoIncrements()
        {
            var (fuel, user) = CreateBasics();
            var sale1 = new Sale(fuel, 5, user, null);
            var sale2 = new Sale(fuel, 5, user, null);
            Assert.NotEqual(sale1.Id, sale2.Id);
        }
    }
}