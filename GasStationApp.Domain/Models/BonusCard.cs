using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class BonusCard : ISaveable
    {
        public string CardNumber { get; private set; }
        public string FullName { get; internal set; }
        public string Phone { get; internal set; }

        private double _bonusBalance;
        public double BonusBalance
        {
            get => _bonusBalance;
            private set => _bonusBalance = value;
        }

        public string LoyaltyLevel { get; private set; }

        public BonusCard(string fullName, string phone)
        {
            FullName = fullName;
            Phone = phone;
            CardNumber = GenerateCardNumber();
            _bonusBalance = 0;
            LoyaltyLevel = "Bronze";
        }

        //Перевірка формату телефону +38(0XX)-XXX-XX-XX
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;
            return Regex.IsMatch(phone, @"^\+38\(0\d{2}\)-\d{3}-\d{2}-\d{2}$");
        }

        //Нарахувати бонуси
        public void AddBonuses(double amount)
        {
            if (amount <= 0)
                return;
            _bonusBalance += amount;
            UpdateLoyaltyLevel();
        }

        //Списати бонуси
        public bool RedeemBonuses(double amount)
        {
            if (amount <= 0)
                return false;
            if (_bonusBalance < amount)
                return false;
            _bonusBalance -= amount;
            return true;
        }

        //Оновити рівень лояльності
        private void UpdateLoyaltyLevel()
        {
            if (_bonusBalance >= 2000)
                LoyaltyLevel = "Gold";
            else if (_bonusBalance >= 500)
                LoyaltyLevel = "Silver";
            //Bronze-Silver при >= 500; Silver-Gold при >= 2000
        }

        //Статичний метод генерації номера бонусної картки
        public static string GenerateCardNumber()
        {
            var rng = new Random();
            return $"{rng.Next(1000, 9999)}-{rng.Next(1000, 9999)}-{rng.Next(1000, 9999)}";
            //Формат XXXX-XXXX-XXXX
        }

        //ISaveable
        public void SaveToJson(string filePath)
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            //реалізується через Administrator.LoadDataFromJson
        }
    }
}