using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class ManagerTests
    {
        private (FuelType, Tank, List<Tank>) CreateSetup()
        {
            var fuel = new FuelType("А-95", 56.50);
            var tank = new Tank(1, fuel, 10000);
            return (fuel, tank, new List<Tank> { tank });
        }

        [Fact]
        public void RefillTank_ValidAmount_ReturnsTrue()
        {
            var (_, tank, _) = CreateSetup();
            var manager = new Manager("mgr1", "pass123");
            var result = manager.RefillTank(tank, 5000);
            Assert.True(result);
            Assert.Equal(5000, tank.CurrentLevel);
        }

        [Fact]
        public void RefillTank_ExceedsCapacity_ReturnsFalse()
        {
            var (_, tank, _) = CreateSetup();
            var manager = new Manager("mgr1", "pass123");
            manager.RefillTank(tank, 9000);
            var result = manager.RefillTank(tank, 5000);
            Assert.False(result);
        }

        [Fact]
        public void GetTanksWithPrices_ReturnsAllTanks()
        {
            var (_, _, tanks) = CreateSetup();
            var manager = new Manager("mgr1", "pass123");
            var result = manager.GetTanksWithPrices(tanks);
            Assert.Equal(tanks.Count, result.Count);
        }

        [Fact]
        public void CreateBonusCard_ValidData_ReturnsCard()
        {
            var manager = new Manager("mgr1", "pass123");
            var cards = new List<BonusCard>();
            var card = manager.CreateBonusCard("Петренко П.П.", "+380671234567", cards);
            Assert.NotNull(card);
            Assert.Equal("Петренко П.П.", card.FullName);
            Assert.Single(cards);
        }

        [Fact]
        public void CreateBonusCard_DuplicatePhone_ReturnsNull()
        {
            var manager = new Manager("mgr1", "pass123");
            var cards = new List<BonusCard>();
            manager.CreateBonusCard("Петренко П.П.", "+380671234567", cards);
            var duplicate = manager.CreateBonusCard("Інший", "+380671234567", cards);
            Assert.Null(duplicate);
        }

        [Fact]
        public void EditBonusCard_ExistingCard_ReturnsTrue()
        {
            var manager = new Manager("mgr1", "pass123");
            var cards = new List<BonusCard>();
            var card = manager.CreateBonusCard("Петренко П.П.", "+380671234567", cards);
            var result = manager.EditBonusCard(card.CardNumber, "Коваленко К.К.", "+380991234567", cards);
            Assert.True(result);
            Assert.Equal("Коваленко К.К.", card.FullName);
        }

        [Fact]
        public void EditBonusCard_NonExistingCard_ReturnsFalse()
        {
            var manager = new Manager("mgr1", "pass123");
            var cards = new List<BonusCard>();
            var result = manager.EditBonusCard("0000-0000-0000", "Тест", "+380", cards);
            Assert.False(result);
        }

        [Fact]
        public void GetGeneralStatistics_ReturnsReport()
        {
            var fuel = new FuelType("А-95", 56.50);
            var op = new Operator("op1", "pass");
            var sales = new List<Sale>
            {
                new Sale(fuel, 10, op, null),
                new Sale(fuel, 20, op, null)
            };
            var manager = new Manager("mgr1", "pass123");
            var report = manager.GetGeneralStatistics(sales);
            Assert.NotNull(report);
            Assert.Equal(2, report.GetTransactionCount());
        }
    }
}