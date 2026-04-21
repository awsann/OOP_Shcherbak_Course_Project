using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class BonusCardTests
    {
        [Fact]
        public void Constructor_ValidData_CreatesCard()
        {
            var card = new BonusCard("Іванов І.І.", "+38(050)-123-45-67");
            Assert.Equal("Іванов І.І.", card.FullName);
            Assert.Equal(0, card.BonusBalance);
            Assert.Equal("Bronze", card.LoyaltyLevel);
        }

        [Fact]
        public void CardNumber_HasCorrectFormat()
        {
            var card = new BonusCard("Тест", "+38(050)-123-45-67");
            Assert.Matches(@"^\d{4}-\d{4}-\d{4}$", card.CardNumber);
        }

        [Fact]
        public void AddBonuses_IncreasesBalance()
        {
            var card = new BonusCard("Тест", "+38(050)-123-45-67");
            card.AddBonuses(100);
            Assert.Equal(100, card.BonusBalance);
        }

        [Fact]
        public void AddBonuses_NegativeAmount_DoesNotChangeBalance()
        {
            var card = new BonusCard("Тест", "+38(050)-123-45-67");
            card.AddBonuses(-100);
            Assert.Equal(0, card.BonusBalance);
        }

        [Fact]
        public void RedeemBonuses_MoreThanBalance_ReturnsFalse()
        {
            var card = new BonusCard("Тест", "+38(050)-123-45-67");
            card.AddBonuses(100);
            var result = card.RedeemBonuses(200);
            Assert.False(result);
        }

        [Fact]
        public void RedeemBonuses_ExactBalance_ReturnsTrue()
        {
            var card = new BonusCard("Тест", "+38(050)-123-45-67");
            card.AddBonuses(100);
            var result = card.RedeemBonuses(100);
            Assert.True(result);
            Assert.Equal(0, card.BonusBalance);
        }

        [Fact]
        public void LoyaltyLevel_UpdatesToBronzeToSilver_At500()
        {
            var card = new BonusCard("Тест", "+38(050)-123-45-67");
            card.AddBonuses(500);
            Assert.Equal("Silver", card.LoyaltyLevel);
        }

        [Fact]
        public void LoyaltyLevel_UpdatesSilverToGold_At2000()
        {
            var card = new BonusCard("Тест", "+38(050)-123-45-67");
            card.AddBonuses(2000);
            Assert.Equal("Gold", card.LoyaltyLevel);
        }

        [Fact]
        public void LoyaltyLevel_DoesNotDowngrade_AfterRedeeming()
        {
            var card = new BonusCard("Тест", "+38(050)-123-45-67");
            card.AddBonuses(500);
            card.RedeemBonuses(300);
            Assert.Equal("Silver", card.LoyaltyLevel);
        }

        [Fact]
        public void IsValidPhone_CorrectFormat_ReturnsTrue()
        {
            Assert.True(BonusCard.IsValidPhone("+38(050)-123-45-67"));
        }

        [Fact]
        public void IsValidPhone_WrongFormat_ReturnsFalse()
        {
            Assert.False(BonusCard.IsValidPhone("+380501234567"));
        }

        [Fact]
        public void IsValidPhone_EmptyString_ReturnsFalse()
        {
            Assert.False(BonusCard.IsValidPhone(""));
        }
    }
}