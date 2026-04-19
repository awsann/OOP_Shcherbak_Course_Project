using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class Administrator : User
    {
        public Administrator(string login, string password)
            : base(login, password) { }

        public override string GetRole() => "Адміністратор";

        //Управління типами палива
        public bool AddFuelType(string name, double price, List<FuelType> fuelTypes)
        {
            if (price <= 0)
                return false;
            if (fuelTypes.Any(f => f.Name == name))
                return false;
            fuelTypes.Add(new FuelType(name, price));
            return true;
        }

        //знайти за Id, перевірити ціну > 0, оновити
        public bool EditFuelType(int id, string newName, double newPrice, List<FuelType> fuelTypes)
        {
            var fuel = fuelTypes.FirstOrDefault(f => f.Id == id);
            if (fuel == null)
                return false;
            if (newPrice <= 0)
                return false;
            fuel.Name = newName;
            fuel.SetPrice(newPrice);
            return true;
        }

        //знайти за Id, видалити
        public bool DeleteFuelType(int id, List<FuelType> fuelTypes)
        {
            var fuel = fuelTypes.FirstOrDefault(f => f.Id == id);
            if (fuel == null)
                return false;
            fuelTypes.Remove(fuel);
            return true;
        }

        //Управління резервуарами
        public bool AddTank(int number, FuelType fuelType, double capacity, List<Tank> tanks)
        {
            if (capacity <= 0)
                return false;
            if (tanks.Any(t => t.Number == number))
                return false;
            tanks.Add(new Tank(number, fuelType, capacity));
            return true;
        }

        //знайти за Id, перевірити що залишок не перевищує нову місткість
        public bool EditTank(int id, double newCapacity, List<Tank> tanks)
        {
            var tank = tanks.FirstOrDefault(t => t.Id == id);
            if (tank == null)
                return false;
            if (newCapacity <= 0)
                return false;
            if (tank.CurrentLevel > newCapacity)
                return false;
            tank.Capacity = newCapacity;
            return true;
        }

        //Управління бонусними картками
        public BonusCard? CreateBonusCard(string fullName, string phone, List<BonusCard> bonusCards)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return null;
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

        //знайти за номером, видалити
        public bool DeleteBonusCard(string cardNumber, List<BonusCard> bonusCards)
        {
            var card = bonusCards.FirstOrDefault(c => c.CardNumber == cardNumber);
            if (card == null)
                return false;
            bonusCards.Remove(card);
            return true;
        }

        //Звітність
        public Report GetFinancialReport(List<Sale> sales)
        {
            return new Report(sales, "Фінансовий");
        }

        //Збереження/завантаження JSON
        public bool SaveDataToJson(string filePath, List<FuelType> fuelTypes, List<Tank> tanks, List<Sale> sales, List<BonusCard> bonusCards)
        {
            if (!fuelTypes.Any() && !tanks.Any() && !sales.Any() && !bonusCards.Any())
                return false;
            try
            {
                var data = new
                {
                    FuelTypes = fuelTypes.Select(f => new { f.Id, f.Name, f.PricePerLiter }),
                    Tanks = tanks.Select(t => new { t.Id, t.Number, FuelTypeId = t.FuelType.Id, t.Capacity, t.CurrentLevel }),
                    Sales = sales.Select(s => new { s.Id, FuelTypeId = s.FuelType.Id, s.Liters, s.PriceAtSaleTime, s.TotalAmount, s.SaleDateTime, PerformedById = s.PerformedBy.Login, s.AccruedBonuses }),
                    BonusCards = bonusCards.Select(c => new { c.CardNumber, c.FullName, c.Phone, c.BonusBalance, c.LoyaltyLevel })
                };
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(filePath, JsonSerializer.Serialize(data, options));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool LoadDataFromJson(string filePath,
            out List<FuelType> fuelTypes,
            out List<Tank> tanks,
            out List<Sale> sales,
            out List<BonusCard> bonusCards)
        {
            fuelTypes = new List<FuelType>();
            tanks = new List<Tank>();
            sales = new List<Sale>();
            bonusCards = new List<BonusCard>();

            try
            {
                var json = File.ReadAllText(filePath);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                //Завантажуємо типи палива
                var fuelDict = new Dictionary<int, FuelType>();
                foreach (var el in root.GetProperty("FuelTypes").EnumerateArray())
                {
                    int oldId = el.GetProperty("Id").GetInt32();
                    var fuel = new FuelType(
                        el.GetProperty("Name").GetString()!,
                        el.GetProperty("PricePerLiter").GetDouble());
                    fuelDict[oldId] = fuel;
                    fuelTypes.Add(fuel);
                }

                //Завантажуємо резервуари
                foreach (var el in root.GetProperty("Tanks").EnumerateArray())
                {
                    int fuelTypeId = el.GetProperty("FuelTypeId").GetInt32();
                    if (!fuelDict.TryGetValue(fuelTypeId, out var fuelType)) continue;
                    var tank = new Tank(
                        el.GetProperty("Number").GetInt32(),
                        fuelType,
                        el.GetProperty("Capacity").GetDouble());
                    double level = el.GetProperty("CurrentLevel").GetDouble();
                    if (level > 0)
                        tank.Refill(level);
                    tanks.Add(tank);
                }

                //Завантажуємо бонусні картки
                foreach (var el in root.GetProperty("BonusCards").EnumerateArray())
                {
                    var card = new BonusCard(
                        el.GetProperty("FullName").GetString()!,
                        el.GetProperty("Phone").GetString()!);
                    double balance = el.GetProperty("BonusBalance").GetDouble();
                    if (balance > 0)
                        card.AddBonuses(balance);
                    bonusCards.Add(card);
                }

                //Завантажуємо продажі
                foreach (var el in root.GetProperty("Sales").EnumerateArray())
                {
                    int fuelTypeId = el.GetProperty("FuelTypeId").GetInt32();
                    if (!fuelDict.TryGetValue(fuelTypeId, out var fuelType)) continue;
                    var performedBy = new Operator(
                        el.GetProperty("PerformedById").GetString()!,
                        "temp");
                    var sale = new Sale(
                        el.GetProperty("Id").GetInt32(),
                        fuelType,
                        el.GetProperty("Liters").GetDouble(),
                        el.GetProperty("PriceAtSaleTime").GetDouble(),
                        el.GetProperty("TotalAmount").GetDouble(),
                        el.GetProperty("SaleDateTime").GetDateTime(),
                        performedBy,
                        el.GetProperty("AccruedBonuses").GetDouble());
                    sales.Add(sale);
                }

                //Скидаємо лічильники Id після завантаження
                if (fuelTypes.Any())
                    FuelType.ResetIdCounter(fuelTypes.Max(f => f.Id) + 1);
                if (tanks.Any())
                    Tank.ResetIdCounter(tanks.Max(t => t.Id) + 1);
                if (sales.Any())
                    Sale.ResetIdCounter(sales.Max(s => s.Id) + 1);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}