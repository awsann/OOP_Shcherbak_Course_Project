using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GasStationApp.Domain.Models
{
    public class FuelType : ISaveable
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public string Name { get; internal set; }

        private double _pricePerLiter;
        public double PricePerLiter
        {
            get => _pricePerLiter;
            private set => _pricePerLiter = value;
        }

        public FuelType(string name, double price)
        {
            Id = _nextId++;
            Name = name;
            _pricePerLiter = price;
        }

        public static void ResetIdCounter(int startFrom)
        {
            _nextId = startFrom;
        }

        //Встановити ціну
        public bool SetPrice(double newPrice)
        {
            if (newPrice <= 0)
                return false;
            _pricePerLiter = newPrice;
            return true;
        }

        //Рядкове представлення
        public override string ToString()
        {
            return $"{Name} — {_pricePerLiter.ToString("F2", CultureInfo.InvariantCulture)} грн/л";
            //Формат: "А-95 — 56.50 грн/л"
        }

        //ISaveable
        public void SaveToJson(string filePath)
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var obj = JsonSerializer.Deserialize<FuelType>(json);
            if (obj != null)
            {
                Name = obj.Name;
                _pricePerLiter = obj.PricePerLiter;
            }
        }
    }
}