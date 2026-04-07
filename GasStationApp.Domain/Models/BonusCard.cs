using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class BonusCard : ISaveable
    {
        public string CardNumber { get; private set; }
        public string FullName { get; private set; }
        public string Phone { get; private set; }

        private double _bonusBalance;
        public double BonusBalance
        {
            get => _bonusBalance;
            private set => _bonusBalance = value;
        }

        public string LoyaltyLevel { get; private set; }

        public void UpdateInfo(string newFullName, string newPhone)
        {
            FullName = newFullName;
            Phone = newPhone;
        }
        public BonusCard(string fullName, string phone)
        {
            FullName = fullName;
            Phone = phone;
            CardNumber = GenerateCardNumber();
            BonusBalance = 0;
            LoyaltyLevel = "Bronze";
        }

        //Нарахувати бонуси
        public void AddBonuses(double amount)
        {
            throw new NotImplementedException();
        }

        //Списати бонуси
        public bool RedeemBonuses(double amount)
        {
            throw new NotImplementedException();
        }

        //Оновити рівень лояльності
        private void UpdateLoyaltyLevel()
        {
            throw new NotImplementedException();
            //Bronze-Silver при >= 500; Silver-Gold при >= 2000
        }

        //Статичний метод генерації номера бонусної картки
        public static string GenerateCardNumber()
        {
            throw new NotImplementedException();
            //Формат: XXXX-XXXX-XXXX
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