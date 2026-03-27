using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //Розрахувати суму
        private double CalculateTotal()
        {
            throw new NotImplementedException();
        }

        //Розрахувати бонуси
        public double CalculateBonuses(string? loyaltyLevel)
        {
            throw new NotImplementedException();
            //Bronze — 3%, Silver — 5%, Gold — 7%
        }

        //ISaveable
        public void SaveToJson(string filePath)
        {
            throw new NotImplementedException();
        }

        public void LoadFromJson(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}