using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class AdministratorTests
    {
        [Fact]
        public void AddFuelType_ValidData_ReturnsTrue()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuelTypes = new List<FuelType>();
            var result = admin.AddFuelType("LPG", 30.00, fuelTypes);
            Assert.True(result);
            Assert.Single(fuelTypes);
        }

        [Fact]
        public void AddFuelType_DuplicateName_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuelTypes = new List<FuelType>();
            admin.AddFuelType("LPG", 30.00, fuelTypes);
            var result = admin.AddFuelType("LPG", 32.00, fuelTypes);
            Assert.False(result);
        }

        [Fact]
        public void AddFuelType_NegativePrice_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuelTypes = new List<FuelType>();
            var result = admin.AddFuelType("А-95", -10, fuelTypes);
            Assert.False(result);
        }

        [Fact]
        public void EditFuelType_ExistingId_ReturnsTrue()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuelTypes = new List<FuelType>();
            admin.AddFuelType("А-95", 56.50, fuelTypes);
            var id = fuelTypes[0].Id;
            var result = admin.EditFuelType(id, "А-95 Преміум", 60.00, fuelTypes);
            Assert.True(result);
            Assert.Equal("А-95 Преміум", fuelTypes[0].Name);
            Assert.Equal(60.00, fuelTypes[0].PricePerLiter);
        }

        [Fact]
        public void EditFuelType_NonExistingId_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuelTypes = new List<FuelType>();
            var result = admin.EditFuelType(9999, "Тест", 50.00, fuelTypes);
            Assert.False(result);
        }

        [Fact]
        public void DeleteFuelType_ExistingId_ReturnsTrue()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuelTypes = new List<FuelType>();
            admin.AddFuelType("Дизель", 50.00, fuelTypes);
            var id = fuelTypes[0].Id;
            var result = admin.DeleteFuelType(id, fuelTypes);
            Assert.True(result);
            Assert.Empty(fuelTypes);
        }

        [Fact]
        public void DeleteFuelType_NonExistingId_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuelTypes = new List<FuelType>();
            var result = admin.DeleteFuelType(9999, fuelTypes);
            Assert.False(result);
        }

        [Fact]
        public void AddTank_ValidData_ReturnsTrue()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuel = new FuelType("А-95", 56.50);
            var tanks = new List<Tank>();
            var result = admin.AddTank(1, fuel, 10000, tanks);
            Assert.True(result);
            Assert.Single(tanks);
        }

        [Fact]
        public void AddTank_DuplicateNumber_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuel = new FuelType("А-95", 56.50);
            var tanks = new List<Tank>();
            admin.AddTank(1, fuel, 10000, tanks);
            var result = admin.AddTank(1, fuel, 5000, tanks);
            Assert.False(result);
        }

        [Fact]
        public void EditTank_ExistingId_ReturnsTrue()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuel = new FuelType("А-95", 56.50);
            var tanks = new List<Tank>();
            admin.AddTank(1, fuel, 10000, tanks);
            var id = tanks[0].Id;
            var result = admin.EditTank(id, 15000, tanks);
            Assert.True(result);
            Assert.Equal(15000, tanks[0].Capacity);
        }

        [Fact]
        public void EditTank_NonExistingId_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var tanks = new List<Tank>();
            var result = admin.EditTank(9999, 15000, tanks);
            Assert.False(result);
        }

        [Fact]
        public void EditTank_NewCapacityLessThanCurrentLevel_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuel = new FuelType("А-95", 56.50);
            var tanks = new List<Tank>();
            admin.AddTank(1, fuel, 10000, tanks);
            tanks[0].Refill(8000);
            var result = admin.EditTank(tanks[0].Id, 5000, tanks);
            Assert.False(result);
        }

        [Fact]
        public void CreateBonusCard_ValidData_ReturnsCard()
        {
            var admin = new Administrator("admin", "Admin123");
            var cards = new List<BonusCard>();
            var card = admin.CreateBonusCard("Сидоренко С.С.", "+380501112233", cards);
            Assert.NotNull(card);
            Assert.Single(cards);
        }

        [Fact]
        public void CreateBonusCard_DuplicatePhone_ReturnsNull()
        {
            var admin = new Administrator("admin", "Admin123");
            var cards = new List<BonusCard>();
            admin.CreateBonusCard("Сидоренко С.С.", "+380501112233", cards);
            var duplicate = admin.CreateBonusCard("Інший", "+380501112233", cards);
            Assert.Null(duplicate);
        }

        [Fact]
        public void EditBonusCard_ExistingCard_ReturnsTrue()
        {
            var admin = new Administrator("admin", "Admin123");
            var cards = new List<BonusCard>();
            var card = admin.CreateBonusCard("Сидоренко С.С.", "+380501112233", cards);
            var result = admin.EditBonusCard(card.CardNumber, "Мороз М.М.", "+380661112233", cards);
            Assert.True(result);
            Assert.Equal("Мороз М.М.", card.FullName);
        }

        [Fact]
        public void DeleteBonusCard_ExistingCard_ReturnsTrue()
        {
            var admin = new Administrator("admin", "Admin123");
            var cards = new List<BonusCard>();
            var card = admin.CreateBonusCard("Сидоренко С.С.", "+380501112233", cards);
            var result = admin.DeleteBonusCard(card.CardNumber, cards);
            Assert.True(result);
            Assert.Empty(cards);
        }

        [Fact]
        public void DeleteBonusCard_NonExistingCard_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var cards = new List<BonusCard>();
            var result = admin.DeleteBonusCard("0000-0000-0000", cards);
            Assert.False(result);
        }

        [Fact]
        public void GetFinancialReport_ReturnsCorrectReport()
        {
            var admin = new Administrator("admin", "Admin123");
            var fuel = new FuelType("А-95", 56.50);
            var op = new Operator("op1", "pass");
            var sales = new List<Sale>
            {
                new Sale(fuel, 10, op, null),
                new Sale(fuel, 20, op, null)
            };
            var report = admin.GetFinancialReport(sales);
            Assert.NotNull(report);
            Assert.Equal(1695.0, report.GetTotalRevenue(), 2);
        }
    }
}