using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class OperatorTests
    {
        private (FuelType, Tank, List<Tank>) CreateSetup()
        {
            var fuel = new FuelType("А-95", 56.50);
            var tank = new Tank(1, fuel, 10000);
            tank.Refill(5000);
            return (fuel, tank, new List<Tank> { tank });
        }

        [Fact]
        public void LogIn_CorrectCredentials_ReturnsTrue_AndSetsShiftStartTime()
        {
            var op = new Operator("op1", "pass123");
            var result = op.LogIn("op1", "pass123");
            Assert.True(result);
            Assert.NotEqual(default, op.ShiftStartTime);
        }

        [Fact]
        public void LogIn_WrongCredentials_ReturnsFalse()
        {
            var op = new Operator("op1", "pass123");
            var result = op.LogIn("op1", "wrongpass");
            Assert.False(result);
        }

        [Fact]
        public void LogOut_DoesNotThrow()
        {
            var op = new Operator("op1", "pass123");
            op.LogIn("op1", "pass123");
            var ex = Record.Exception(() => op.LogOut());
            Assert.Null(ex);
        }

        [Fact]
        public void MakeSale_ValidData_ReturnsSale()
        {
            var (fuel, tank, tanks) = CreateSetup();
            var op = new Operator("op1", "pass123");
            var sale = op.MakeSale(fuel, 10, null, tanks);
            Assert.NotNull(sale);
            Assert.Equal(565.0, sale.TotalAmount);
        }

        [Fact]
        public void MakeSale_NotEnoughFuel_ReturnsNull()
        {
            var (fuel, tank, tanks) = CreateSetup();
            var op = new Operator("op1", "pass123");
            var sale = op.MakeSale(fuel, 99999, null, tanks);
            Assert.Null(sale);
        }

        [Fact]
        public void MakeSale_ZeroLiters_ReturnsNull()
        {
            var (fuel, tank, tanks) = CreateSetup();
            var op = new Operator("op1", "pass123");
            var sale = op.MakeSale(fuel, 0, null, tanks);
            Assert.Null(sale);
        }

        [Fact]
        public void MakeSale_LowFuelLevel_ReturnsNull()
        {
            var fuel = new FuelType("А-95", 56.50);
            var tank = new Tank(1, fuel, 10000);
            tank.Refill(1000);
            var tanks = new List<Tank> { tank };
            var op = new Operator("op1", "pass123");
            var sale = op.MakeSale(fuel, 10, null, tanks);
            Assert.Null(sale);
        }

        [Fact]
        public void MakeSale_WithBonusCard_AccruesBonuses()
        {
            var (fuel, tank, tanks) = CreateSetup();
            var op = new Operator("op1", "pass123");
            var card = new BonusCard("Тест", "+380501234567");
            var sale = op.MakeSale(fuel, 10, card, tanks);
            Assert.NotNull(sale);
            Assert.True(card.BonusBalance > 0);
        }

        [Fact]
        public void GetTanksWithPrices_ReturnsAllTanks()
        {
            var (_, _, tanks) = CreateSetup();
            var op = new Operator("op1", "pass123");
            var result = op.GetTanksWithPrices(tanks);
            Assert.Equal(tanks.Count, result.Count);
        }

        [Fact]
        public void FindClient_ExistingCard_ReturnsCard()
        {
            var op = new Operator("op1", "pass123");
            var card = new BonusCard("Іванов І.І.", "+380501234567");
            var cards = new List<BonusCard> { card };
            var found = op.FindClient(card.CardNumber, cards);
            Assert.NotNull(found);
            Assert.Equal(card.CardNumber, found.CardNumber);
        }

        [Fact]
        public void FindClient_NonExistingCard_ReturnsNull()
        {
            var op = new Operator("op1", "pass123");
            var cards = new List<BonusCard>();
            var found = op.FindClient("0000-0000-0000", cards);
            Assert.Null(found);
        }

        [Fact]
        public void RedeemBonuses_SufficientBalance_ReturnsTrue()
        {
            var op = new Operator("op1", "pass123");
            var card = new BonusCard("Тест", "+380");
            card.AddBonuses(200);
            var result = op.RedeemBonuses(card, 100);
            Assert.True(result);
            Assert.Equal(100, card.BonusBalance);
        }

        [Fact]
        public void RedeemBonuses_InsufficientBalance_ReturnsFalse()
        {
            var op = new Operator("op1", "pass123");
            var card = new BonusCard("Тест", "+380");
            card.AddBonuses(50);
            var result = op.RedeemBonuses(card, 200);
            Assert.False(result);
        }

        [Fact]
        public void GetShiftReport_ReturnsSalesOnlyForThisOperator()
        {
            var fuel = new FuelType("А-95", 56.50);
            var op1 = new Operator("op1", "pass123");
            var op2 = new Operator("op2", "pass456");
            var allSales = new List<Sale>
            {
               new Sale(fuel, 10, op1, null),
               new Sale(fuel, 5, op2, null),
               new Sale(fuel, 8, op1, null)
            };
            var report = op1.GetShiftReport(allSales);
            Assert.Equal(2, report.FilterByUser(op1).Count);
        }
    }
}