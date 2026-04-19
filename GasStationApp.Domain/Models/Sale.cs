using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class Sale : ISaveable
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public FuelType FuelType { get; private set; }
        public double Liters { get; private set; }
        public double PriceAtSaleTime { get; private set; }
        public double TotalAmount { get; private set; }
        public DateTime SaleDateTime { get; private set; }
        public User PerformedBy { get; private set; }
        public BonusCard? BonusCard { get; private set; }
        public double AccruedBonuses { get; private set; }

        //Основний конструктор для нового продажу
        public Sale(FuelType fuelType, double liters, User performedBy, BonusCard? bonusCard)
        {
            Id = _nextId++;
            FuelType = fuelType;
            Liters = liters;
            PriceAtSaleTime = fuelType.PricePerLiter;
            PerformedBy = performedBy;
            BonusCard = bonusCard;
            SaleDateTime = DateTime.Now;
            TotalAmount = CalculateTotal();
            AccruedBonuses = CalculateBonuses(bonusCard?.LoyaltyLevel);
        }

        //Конструктор для відновлення з JSON
        public Sale(int id, FuelType fuelType, double liters, double priceAtSaleTime,
                    double totalAmount, DateTime saleDateTime, User performedBy, double accruedBonuses)
        {
            Id = id;
            FuelType = fuelType;
            Liters = liters;
            PriceAtSaleTime = priceAtSaleTime;
            TotalAmount = totalAmount;
            SaleDateTime = saleDateTime;
            PerformedBy = performedBy;
            BonusCard = null;
            AccruedBonuses = accruedBonuses;
        }

        public static void ResetIdCounter(int startFrom)
        {
            _nextId = startFrom;
        }

        //Розрахувати суму
        private double CalculateTotal()
        {
            return Liters * PriceAtSaleTime;
        }

        //Розрахувати бонуси
        public double CalculateBonuses(string? loyaltyLevel)
        {
            return loyaltyLevel switch
            {
                "Bronze" => TotalAmount * 0.03,
                "Silver" => TotalAmount * 0.05,
                "Gold" => TotalAmount * 0.07,
                _ => 0
                //Bronze — 3%, Silver — 5%, Gold — 7%
            };
        }

        //ISaveable
        public void SaveToJson(string filePath)
        {
            var json = JsonSerializer.Serialize(new
            {
                Id,
                Liters,
                PriceAtSaleTime,
                TotalAmount,
                SaleDateTime,
                AccruedBonuses
            });
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            //реалізується через Administrator.LoadDataFromJson
        }
    }
}