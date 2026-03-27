using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class Operator : User
    {
        //Характеристика(таблиця 2.7)
        public DateTime ShiftStartTime { get; private set; }

        public Operator(string login, string password)
            : base(login, password) { }

        public override string GetRole() => "Оператор";

        public override bool LogIn(string login, string password)
        {
            throw new NotImplementedException();
            //При вході встановлювати ShiftStartTime = DateTime.Now
        }

        public override void LogOut()
        {
            throw new NotImplementedException();
            //Фіксація виходу для звітності по змінах
        }

        //Продаж палива
        public Sale? MakeSale(FuelType fuelType, double liters, BonusCard? bonusCard, List<Tank> tanks)
        {
            throw new NotImplementedException();
        }

        //Перегляд цін та залишків
        public List<Tank> GetTanksWithPrices(List<Tank> tanks)
        {
            throw new NotImplementedException();
        }

        //Пошук клієнта за бонусною карткою
        public BonusCard? FindClient(string cardNumber, List<BonusCard> bonusCards)
        {
            throw new NotImplementedException();
        }

        //Списання бонусів
        public bool RedeemBonuses(BonusCard bonusCard, double amount)
        {
            throw new NotImplementedException();
        }

        //Звіт за зміну
        public Report GetShiftReport(List<Sale> allSales)
        {
            throw new NotImplementedException();
        }
    }
}