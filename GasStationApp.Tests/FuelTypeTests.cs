using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class FuelTypeTests
    {
        [Fact]
        public void Constructor_ValidData_CreatesFuelType()
        {
            var fuel = new FuelType("А-95", 56.50);
            Assert.Equal("А-95", fuel.Name);
            Assert.Equal(56.50, fuel.PricePerLiter);
        }

        [Fact]
        public void SetPrice_ValidPrice_ReturnsTrue()
        {
            var fuel = new FuelType("Дизель", 50.00);
            var result = fuel.SetPrice(52.00);
            Assert.True(result);
            Assert.Equal(52.00, fuel.PricePerLiter);
        }

        [Fact]
        public void SetPrice_NegativePrice_ReturnsFalse()
        {
            var fuel = new FuelType("А-95", 56.50);
            var result = fuel.SetPrice(-5);
            Assert.False(result);
        }

        [Fact]
        public void SetPrice_ZeroPrice_ReturnsFalse()
        {
            var fuel = new FuelType("А-95", 56.50);
            var result = fuel.SetPrice(0);
            Assert.False(result);
        }

        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            var fuel = new FuelType("А-95", 56.50);
            Assert.Equal("А-95 — 56.50 грн/л", fuel.ToString());
        }

        [Fact]
        public void Id_AutoIncrements()
        {
            var fuel1 = new FuelType("А-92", 54.00);
            var fuel2 = new FuelType("LPG", 30.00);
            Assert.NotEqual(fuel1.Id, fuel2.Id);
        }
    }
}