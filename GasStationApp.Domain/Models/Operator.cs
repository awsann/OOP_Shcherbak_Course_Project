using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class Operator : User
    {
        //час початку зміни, встановлюється при вході
        public DateTime ShiftStartTime { get; private set; }

        public Operator(string login, string password)
            : base(login, password) { }

        public override string GetRole() => "Оператор";

        public override bool LogIn(string login, string password)
        {
            bool success = base.LogIn(login, password);
            if (success)
                ShiftStartTime = DateTime.Now;
            return success;
        }

        //Фіксація виходу для звітності по змінах
        public override void LogOut()
        {
            //факт виходу зафіксовано
        }

        //Продаж палива
        public Sale? MakeSale(FuelType fuelType, double liters, BonusCard? bonusCard, List<Tank> tanks)
        {
            if (liters <= 0)
                return null;
            var tank = tanks.FirstOrDefault(t => t.FuelType == fuelType);
            if (tank == null)
                return null;
            if (tank.IsLowLevel())
                return null;
            if (!tank.Decrease(liters))
                return null;
            var sale = new Sale(fuelType, liters, this, bonusCard);
            //нарахувати бонуси на картку якщо є
            if (bonusCard != null)
                bonusCard.AddBonuses(sale.AccruedBonuses);
            return sale;
        }

        //Перегляд цін та залишків
        public List<Tank> GetTanksWithPrices(List<Tank> tanks)
        {
            return tanks.ToList();
        }

        //Пошук клієнта за бонусною карткою
        public BonusCard? FindClient(string cardNumber, List<BonusCard> bonusCards)
        {
            return bonusCards.FirstOrDefault(c => c.CardNumber == cardNumber);
        }

        //Списання бонусів
        public bool RedeemBonuses(BonusCard bonusCard, double amount)
        {
            return bonusCard.RedeemBonuses(amount);
        }

        //Звіт за зміну
        public Report GetShiftReport(List<Sale> allSales)
        {
            var mySales = allSales.Where(s => s.PerformedBy == this).ToList();
            return new Report(mySales, "Зміна оператора");
        }
    }
}