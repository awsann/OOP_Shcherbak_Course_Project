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
            return tank.Refill(liters);
        }

        // Перегляд цін та залишків
        public List<Tank> GetTanksWithPrices(List<Tank> tanks)
        {
            return tanks.ToList();
        }

        //Бонусні картки
        public BonusCard? CreateBonusCard(string fullName, string phone, List<BonusCard> bonusCards)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return null;
            //перевірка унікальності телефону
            if (bonusCards.Any(c => c.Phone == phone))
                return null;
            var card = new BonusCard(fullName, phone);
            bonusCards.Add(card);
            return card;
        }

        //знайти за номером, оновити лише контактні дані
        public bool EditBonusCard(string cardNumber, string newFullName, string newPhone, List<BonusCard> bonusCards)
        {
            var card = bonusCards.FirstOrDefault(c => c.CardNumber == cardNumber);
            if (card == null)
                return false;
            card.FullName = newFullName;
            card.Phone = newPhone;
            return true;
        }

        //Загальна статистика
        public Report GetGeneralStatistics(List<Sale> sales)
        {
            return new Report(sales, "Загальна статистика");
        }
    }
}