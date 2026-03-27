using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class Manager : User
    {
        public Manager(string login, string password)
            : base(login, password) { }

        public override string GetRole() => "Менеджер";

        //Поповнення резервуару
        public bool RefillTank(Tank tank, double liters)
        {
            throw new NotImplementedException();
        }

        // Перегляд цін та залишків
        public List<Tank> GetTanksWithPrices(List<Tank> tanks)
        {
            throw new NotImplementedException();
        }

        //Бонусні картки
        public BonusCard? CreateBonusCard(string fullName, string phone, List<BonusCard> bonusCards)
        {
            throw new NotImplementedException();
        }

        public bool EditBonusCard(string cardNumber, string newFullName, string newPhone, List<BonusCard> bonusCards)
        {
            throw new NotImplementedException();
        }

        //Загальна статистика
        public Report GetGeneralStatistics(List<Sale> sales)
        {
            throw new NotImplementedException();
        }
    }
}