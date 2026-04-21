using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class TankTests
    {
        private Tank CreateTank(double capacity = 10000)
        {
            var fuel = new FuelType("А-95", 56.50);
            return new Tank(1, fuel, capacity);
        }

        [Fact]
        public void Constructor_ValidData_CreatesTank()
        {
            var tank = CreateTank();
            Assert.Equal(0, tank.CurrentLevel);
            Assert.Equal(10000, tank.Capacity);
        }

        [Fact]
        public void Refill_ValidAmount_IncreasesLevel()
        {
            var tank = CreateTank();
            var result = tank.Refill(5000);
            Assert.True(result);
            Assert.Equal(5000, tank.CurrentLevel);
        }

        [Fact]
        public void Refill_ExceedsCapacity_ReturnsFalse()
        {
            var tank = CreateTank(10000);
            tank.Refill(8000);
            var result = tank.Refill(5000);
            Assert.False(result);
        }

        [Fact]
        public void Refill_NegativeAmount_ReturnsFalse()
        {
            var tank = CreateTank();
            var result = tank.Refill(-100);
            Assert.False(result);
        }

        [Fact]
        public void Decrease_ValidAmount_DecreasesLevel()
        {
            var tank = CreateTank();
            tank.Refill(5000);
            var result = tank.Decrease(1000);
            Assert.True(result);
            Assert.Equal(4000, tank.CurrentLevel);
        }

        [Fact]
        public void Decrease_MoreThanAvailable_ReturnsFalse()
        {
            var tank = CreateTank();
            tank.Refill(500);
            var result = tank.Decrease(1000);
            Assert.False(result);
        }

        [Fact]
        public void IsLowLevel_Below10Percent_ReturnsTrue()
        {
            var tank = CreateTank(10000);
            tank.Refill(500); 
            Assert.True(tank.IsLowLevel());
        }

        [Fact]
        public void IsLowLevel_ExactlyAt10Percent_ReturnsFalse()
        {
            var tank = CreateTank(10000);
            tank.Refill(1000);
            Assert.False(tank.IsLowLevel());
        }

        [Fact]
        public void GetFillPercentage_Returns50_WhenHalfFull()
        {
            var tank = CreateTank(10000);
            tank.Refill(5000);
            Assert.Equal(50.0, tank.GetFillPercentage());
        }

        [Fact]
        public void LowFuelWarning_EventFired_WhenLevelLow()
        {
            var tank = CreateTank(10000);
            tank.Refill(5000);
            bool eventFired = false;
            tank.LowFuelWarning += (t, level) => eventFired = true;
            tank.Decrease(4200);
            Assert.True(eventFired);
        }
    }
}